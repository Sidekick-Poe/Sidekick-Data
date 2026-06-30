using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Data.BaseItemTypes;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.GraphQl.Models;
using Sidekick.Data.Cli.ItemClasses.Responses;
using Sidekick.Data.Cli.ItemDefinitions.Responses;
using Sidekick.Data.ItemDefinitions;
using Sidekick.Data.Languages;
using Sidekick.Data.Ninja;
using Sidekick.Data.Trade;

namespace Sidekick.Data.Cli.ItemDefinitions;

public class ItemDefinitionBuilder(
    ILogger<ItemDefinitionBuilder> logger,
    DbContextOptions<DataDbContext> dbContextOptions,
    GraphQlClient graphQlClient)
{
    private static readonly Dictionary<string, string> BaseItemToTradeItemMappings = new()
    {
        { "Metadata/Items/Maps/MapKeyTier1", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier2", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier3", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier4", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier5", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier6", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier7", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier8", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier9", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier10", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier11", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier12", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier13", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier14", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier15", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/Maps/MapKeyTier16", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/TradeProxy/BlightedMap", "Metadata/Items/TradeProxy/MapKey" },
        { "Metadata/Items/TradeProxy/UberBlightedMap", "Metadata/Items/TradeProxy/MapKey" },
    };

    public async Task Build(GameType game, IGameLanguage language)
    {
        await using var dbContext = new DataDbContext(dbContextOptions);
        dbContext.ItemDefinitions.RemoveRange(
            dbContext.ItemDefinitions.Where(x => x.Game == game && x.Language == language.Code));
        await dbContext.SaveChangesAsync();

        // Load data from database (Trade, Ninja) and GraphQL (BaseItems)
        var tradeItems = await dbContext.TradeItems
            .Include(x => x.Category)
            .Include(x => x.StaticItem)
            .Where(x => x.Game == game && x.Language == language.Code)
            .ToListAsync();
        var baseItems = await GetBaseItems(dbContext, game, language);
        var uniqueItems = await GetOrCreateUniqueItems(game, language);
        var ninjaStashItems = await dbContext.NinjaStashItems
            .Where(x => x.Game == game)
            .Include(x => x.TradeStats)
            .Include(x => x.MutatedStats)
            .ToListAsync();
        var ninjaExchangeItems = await dbContext.NinjaExchangeItems
            .Where(x => x.Game == game)
            .ToListAsync();

        // 1. Process trade items - match with base/unique/ninja
        foreach (var tradeItem in tradeItems)
        {
            var baseItem = FindMatchingBaseItem(baseItems, tradeItem);
            var uniqueItem = uniqueItems.FirstOrDefault(x => x.Name == tradeItem.Name);

            dbContext.ItemDefinitions.Add(new ItemDefinitionEntity
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                Language = language.Code,
                TradeItemId = tradeItem.SidekickId,
                BaseItemId = baseItem?.SidekickId,
                UniqueItemId = uniqueItem?.SidekickId,
                NamePattern = GetNamePatternString(tradeItem),
                TypePattern = GetTypePatternString(tradeItem.Type, uniqueItem != null),
                TextPattern = GetTextPatternString(tradeItem, uniqueItem != null),
                NinjaExchangeItem = FindMatchingNinjaExchangeItem(ninjaExchangeItems, tradeItem),
                NinjaStashItems = FindMatchingNinjaStashItems(ninjaStashItems, tradeItem, baseItem, uniqueItem),
            });
        }

        // 2. Process base items that don't have a trade item
        foreach (var baseItem in baseItems)
        {
            if (definitions.Any(x => x.BaseItemId == baseItem.SidekickId)) continue;

            var tradeItem = FindMatchingTradeItem(baseItem, tradeItems, baseItems);
            var (ninjaStashItem, ninjaExchangeItem) =
                FindMatchingNinjaItems(tradeItem, baseItem, null, ninjaStashItems, ninjaExchangeItems);

            var entity = new ItemDefinitionEntity
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                Language = language.Code,
                TradeItemId = tradeItem?.SidekickId,
                BaseItemId = baseItem.SidekickId,
                NinjaStashItemId = ninjaStashItem?.SidekickId,
                NinjaExchangeItemId = ninjaExchangeItem?.SidekickId,
                TypePattern = tradeItem == null ? null : GetTypePatternString(baseItem.Name, false),
            };
            definitions.Add(entity);
        }

        // Upsert: remove existing for this game/language, then add new
        var existing = await dbContext.ItemDefinitions
            .Where(x => x.Game == game && x.Language == language.Code)
            .ToListAsync();

        if (existing.Count > 0)
        {
            dbContext.ItemDefinitions.RemoveRange(existing);
        }

        await dbContext.ItemDefinitions.AddRangeAsync(definitions);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("[ItemDefinitionBuilder] Saved {Count} item definitions for {Game}/{Lang}",
            definitions.Count, game, language.Code);
    }

    private BaseItemType? FindMatchingBaseItem(List<BaseItemType> baseItems, TradeItem tradeItem)
    {
        BaseItemType? baseItem = null;
        if (!string.IsNullOrEmpty(tradeItem.Text)) baseItem ??= baseItems.FirstOrDefault(x => x.Name == tradeItem.Text);
        baseItem ??= baseItems.FirstOrDefault(x => x.Name == tradeItem.Type);
        return baseItem;
    }

    private TradeItem? FindMatchingTradeItem(BaseItemType baseItem, List<TradeItem> tradeItems,
        List<BaseItemType> baseItems)
    {
        var tradeItem = tradeItems.FirstOrDefault(x => x.Type == baseItem.Name);
        tradeItem ??= tradeItems.FirstOrDefault(x => x.Name == baseItem.Name);
        tradeItem ??= tradeItems.FirstOrDefault(x => x.Text == baseItem.Name);

        if (tradeItem == null && baseItem.Id != null &&
            BaseItemToTradeItemMappings.TryGetValue(baseItem.Id, out var mapping))
        {
            var baseItemMapping = baseItems.FirstOrDefault(x => x.Id == mapping);
            if (baseItemMapping != null)
            {
                tradeItem ??= tradeItems.FirstOrDefault(x => x.Type == baseItemMapping.Name);
                tradeItem ??= tradeItems.FirstOrDefault(x => x.Name == baseItemMapping.Name);
                tradeItem ??= tradeItems.FirstOrDefault(x => x.Text == baseItemMapping.Name);
            }
        }

        return tradeItem;
    }

    private NinjaExchangeItem? FindMatchingNinjaExchangeItem(
        List<NinjaExchangeItem> ninjaExchangeItems,
        TradeItem? tradeItem)
    {
        if (tradeItem?.StaticItem == null) return null;
        return ninjaExchangeItems.FirstOrDefault(x => x.Id == tradeItem.StaticItem.Id);
    }

    private IEnumerable<NinjaStashItem> FindMatchingNinjaStashItems(
        List<NinjaStashItem> ninjaStashItems,
        TradeItem? tradeItem,
        BaseItemType? baseItem,
        UniqueItemEntity? uniqueItem)
    {
        if (language.Code != gameLanguageProvider.InvariantLanguage.Code) yield break;

        foreach (var ninjaItem in ninjaStashItems)
        {
            // Unique items, support for foulborn uniques
            if (uniqueItem != null)
            {
                if (ninjaItem.Name != null &&
                    (
                        ninjaItem.Name == uniqueItem.Name ||
                        ninjaItem.Name == $"Foulborn {uniqueItem.Name}"
                    )) yield return ninjaItem;
            }

            // Text comparison, support for transfigured gems
            else if (tradeItem is
                     {
                         Text: not null,
                         Name: null,
                     })
            {
                if (ninjaItem.Name != null &&
                    ninjaItem.Name == tradeItem.Text) yield return ninjaItem;
            }

            // Base items, support for maps
            else if (baseItem != null &&
                     ninjaItem.Name != null &&
                     (
                         ninjaItem.Name == baseItem.Name ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "Al-Hezmin Map (Tier 16)") ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "Baran Map (Tier 16)") ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "Drox Map (Tier 16)") ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "Veritania Map (Tier 16)") ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "The Constrictor Map (Tier 16)") ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "The Enslaver Map (Tier 16)") ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "The Eradicator Map (Tier 16)") ||
                         (baseItem.Name == "Map (Tier 16)" && ninjaItem.Name == "The Purifier Map (Tier 16)") ||
                         (baseItem.Name == "Blighted Map" && ninjaItem.Name.StartsWith("Blighted Map (Tier ")) ||
                         (baseItem.Name == "Blight-ravaged Map" && ninjaItem.Name == "Blight-ravaged Map (Tier 16)")
                     )) yield return ninjaItem;

            // Cluster jewel support
            else if (baseItem != null &&
                     ninjaItem.BaseType != null &&
                     ninjaItem.Type != "UniqueJewel" &&
                     ninjaItem.BaseType == baseItem.Name &&
                     baseItem.Name is "Small Cluster Jewel" or "Medium Cluster Jewel" or "Large Cluster Jewel")
                yield return ninjaItem;

            // Name match
            else if (ninjaItem.Name == (baseItem?.Name ?? tradeItem?.Type)) yield return ninjaItem;
        }
    }


    private async Task<List<BaseItemType>> GetBaseItems(DataDbContext dbContext, GameType game,
        IGameLanguage language)
    {
        dbContext.BaseItems.RemoveRange(dbContext.BaseItems.Where(x => x.Game == game && x.Language == language.Code));
        await dbContext.SaveChangesAsync();

        // Fetch from GraphQL
        var lang = GraphQlClient.GetLanguageName(language.Code);
        var prefix = game == GameType.PathOfExile1 ? "poe1" : "poe2";

        var query = $@"
          query {{
            {prefix}_baseItemTypes(lang: ""{lang}"") {{
              {GraphQlItemClass.GraphQlQueryProperties}
            }}
          }}";
        var result = await graphQlClient.QueryAsync<GraphQlBaseItemTypesResponse>(query);
        var graphItems = result?.Poe1BaseItemTypes ?? result?.Poe2BaseItemTypes;
        if (graphItems == null || graphItems.Count == 0)
        {
            logger.LogWarning("[ItemDefinitionBuilder] No base items from GraphQL for {Game}/{Lang}", game,
                language.Code);
            return [];
        }

        var armourQuery = $@"
          query {{
            {prefix}_armourTypes(lang: ""{lang}"") {{
              {GraphQlArmourType.GraphQlQueryProperties}
            }}
          }}";
        var armourResult = await graphQlClient.QueryAsync<GraphQlArmourTypesResponse>(armourQuery);
        var armourItems = armourResult?.Poe1ArmourTypes ?? armourResult?.Poe2ArmourTypes;
        var armourLookup = armourItems?.ToDictionary(a => a.BaseItem.Id, a => a) ?? new();

        var requirementQuery = $@"
          query {{
            {prefix}_componentAttributeRequirements(lang: ""{lang}"") {{
              {GraphQlAttributeRequirement.GraphQlQueryProperties}
            }}
          }}";
        var requirementResult = await graphQlClient.QueryAsync<GraphQlAttributeRequirementsResponse>(requirementQuery);
        var requirementItems =
            requirementResult?.Poe1AttributeRequirements ?? requirementResult?.Poe2AttributeRequirements;
        var requirementLookup = requirementItems?.ToDictionary(a => a.BaseItemId, a => a) ?? new();

        var entities = new List<BaseItemType>();
        foreach (var item in graphItems)
        {
            if (string.IsNullOrEmpty(item.Name)) continue;
            var entity = new BaseItemType
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                Language = language.Code,
                Id = item.Id,
                Name = item.Name,
                DropLevel = item.DropLevel ?? 0,
            };

            if (armourLookup.TryGetValue(item.Id, out var armour))
            {
                entity.ArmourMin = armour.ArmourMin;
                entity.ArmourMax = armour.ArmourMax;
                entity.EnergyShieldMin = armour.EnergyShieldMin;
                entity.EnergyShieldMax = armour.EnergyShieldMax;
                entity.EvasionMin = armour.EvasionMin;
                entity.EvasionMax = armour.EvasionMax;
                entity.WardMin = armour.WardMin;
                entity.WardMax = armour.WardMax;
            }

            if (requirementLookup.TryGetValue(item.Id, out var requirement))
            {
                entity.RequiresDexterity = requirement.RequiresDexterity;
                entity.RequiresIntelligence = requirement.RequiresIntelligence;
                entity.RequiresStrength = requirement.RequiresStrength;
            }

            entities.Add(entity);
        }

        dbContext.BaseItems.AddRange(entities);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("[ItemDefinitionBuilder] Created {Count} base items for {Game}/{Lang}", entities.Count,
            game, language.Code);
        return entities;
    }

    private async Task<List<UniqueItemEntity>> GetUniqueItems(DataDbContext dbContext, GameType game, IGameLanguage language)
    {
        // Unique items not available from GraphQL API yet
        logger.LogDebug("[ItemDefinitionBuilder] Unique items not available from GraphQL API for {Game}/{Lang}.", game,
            language.Code);
        return [];
    }

    private static string? GetNamePatternString(TradeItem? tradeItem)
    {
        if (string.IsNullOrEmpty(tradeItem?.Name)) return null;
        return $"^{Regex.Escape(tradeItem.Name)}|{Regex.Escape(tradeItem.Name)}$";
    }

    private static string? GetTypePatternString(string? type, bool isUnique)
    {
        if (string.IsNullOrEmpty(type) || isUnique) return null;
        return $@"(?<!\p{{L}}){Regex.Escape(type)}(?!\p{{L}})";
    }

    private static string? GetTextPatternString(TradeItem? tradeItem, bool isUnique)
    {
        if (string.IsNullOrEmpty(tradeItem?.Text) || tradeItem.Text == tradeItem.Type || isUnique) return null;
        return Regex.Escape(tradeItem.Text);
    }

    private string GetInvariantKey(UniqueItemEntity? uniqueItem, TradeItem? tradeItem, BaseItemType? baseItem)
    {
        var key = new StringBuilder();
        if (!string.IsNullOrEmpty(uniqueItem?.Id)) key.Append(uniqueItem.Id);
        if (!string.IsNullOrEmpty(tradeItem?.StaticItem?.Id)) key.Append(tradeItem.StaticItem.Id);
        if (!string.IsNullOrEmpty(tradeItem?.Discriminator)) key.Append(tradeItem.Discriminator);
        if (!string.IsNullOrEmpty(baseItem?.Id)) key.Append(baseItem.Id);
        return key.ToString();
    }
}