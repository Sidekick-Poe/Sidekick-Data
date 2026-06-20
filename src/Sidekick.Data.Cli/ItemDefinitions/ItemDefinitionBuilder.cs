using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.GraphQl.Models;
using Sidekick.Data.Cli.Ninja;
using Sidekick.Data.Cli.Trade.Models;
using Sidekick.Data.ItemDefinitions;
using Sidekick.Data.Languages;

namespace Sidekick.Data.Cli.ItemDefinitions;

public class ItemDefinitionBuilder(
    ILogger<ItemDefinitionBuilder> logger,
    IOptions<SidekickConfiguration> configuration,
    DataProvider dataProvider,
    IGameLanguageProvider gameLanguageProvider,
    GraphQlClient graphQlClient,
    NinjaDownloader ninjaDownloader)
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

    public async Task Build(IGameLanguage language)
    {
        try
        {
            await BuildForGame(GameType.PathOfExile1, language);
            await BuildForGame(GameType.PathOfExile2, language);
        }
        catch (Exception ex)
        {
            if (configuration.Value.ApplicationType == SidekickApplicationType.DataBuilder || configuration.Value.ApplicationType == SidekickApplicationType.Test)
                throw;
            logger.LogError(ex, "Failed to build items data.");
        }
    }

    private async Task BuildForGame(GameType game, IGameLanguage language)
    {
        var tradeItems = await GetTradeItems(game, language);
        var baseItems = await GetBaseItems(game, language);
        var uniqueItems = await GetUniqueItems(game, language);
        var ninjaItems = await ninjaDownloader.GetDefinitions(game);

        var list = new List<ItemDefinition>();
        foreach (var tradeItem in tradeItems)
        {
            var baseItem = baseItems.FirstOrDefault(x => !string.IsNullOrEmpty(tradeItem.Text) && x.Name == tradeItem.Text);
            baseItem ??= baseItems.FirstOrDefault(x => x.Name == tradeItem.Type);
            var uniqueItem = uniqueItems.FirstOrDefault(x => x.Name == tradeItem.Name);
            list.Add(new ItemDefinition
            {
                TradeItem = tradeItem, BaseItem = baseItem, UniqueItem = uniqueItem,
                NinjaItems = GetNinjaItems(tradeItem, baseItem, uniqueItem),
                NamePattern = GetNamePattern(tradeItem),
                TypePattern = GetTypePattern(tradeItem.Type, uniqueItem != null),
                TextPattern = GetTextPattern(tradeItem, uniqueItem != null),
            });
        }

        foreach (var baseItem in baseItems)
        {
            if (list.Any(x => x.BaseItem == baseItem)) continue;
            var tradeItem = tradeItems.FirstOrDefault(x => x.Type == baseItem.Name);
            tradeItem ??= tradeItems.FirstOrDefault(x => x.Name == baseItem.Name);
            tradeItem ??= tradeItems.FirstOrDefault(x => x.Text == baseItem.Name);
            if (tradeItem == null && baseItem.Id != null && BaseItemToTradeItemMappings.TryGetValue(baseItem.Id, out var mapping))
            {
                var baseItemMapping = baseItems.FirstOrDefault(x => x.Id == mapping);
                if (baseItemMapping != null)
                {
                    tradeItem ??= tradeItems.FirstOrDefault(x => x.Type == baseItemMapping.Name);
                    tradeItem ??= tradeItems.FirstOrDefault(x => x.Name == baseItemMapping.Name);
                    tradeItem ??= tradeItems.FirstOrDefault(x => x.Text == baseItemMapping.Name);
                }
            }
            list.Add(new ItemDefinition
            {
                TradeItem = tradeItem, BaseItem = baseItem,
                NinjaItems = GetNinjaItems(tradeItem, baseItem, null),
                TypePattern = tradeItem == null ? null : GetTypePattern(baseItem.Name, false),
            });
        }

        await dataProvider.Write(game, DataType.Items, language, list);

        return;

        Regex? GetNamePattern(TradeItemDefinition? tradeItem)
        {
            if (string.IsNullOrEmpty(tradeItem?.Name)) return null;
            return new Regex($"^{Regex.Escape(tradeItem.Name)}|{Regex.Escape(tradeItem.Name)}$");
        }

        Regex? GetTypePattern(string? type, bool isUnique)
        {
            if (string.IsNullOrEmpty(type) || isUnique) return null;
            return new Regex($@"(?<!\\p{{L}}){Regex.Escape(type)}(?!\\p{{L}})");
        }

        Regex? GetTextPattern(TradeItemDefinition? tradeItem, bool isUnique)
        {
            if (string.IsNullOrEmpty(tradeItem?.Text) || tradeItem.Text == tradeItem.Type || isUnique) return null;
            return new Regex(Regex.Escape(tradeItem.Text));
        }

        List<NinjaItemDefinition>? GetNinjaItems(TradeItemDefinition? tradeItem, BaseItemDefinition? baseItem, UniqueItemDefinition? uniqueItem)
        {
            if (language.Code != gameLanguageProvider.InvariantLanguage.Code) return null;
            var result = new List<NinjaItemDefinition>();
            foreach (var ninjaItem in ninjaItems)
            {
                if (tradeItem != null && ninjaItem.Exchange?.Id != null && ninjaItem.Exchange.Id == tradeItem.Id) result.Add(ninjaItem);
                else if (uniqueItem != null && ninjaItem.Stash?.Name != null && (ninjaItem.Stash.Name == uniqueItem.Name || ninjaItem.Stash.Name == $"Foulborn {uniqueItem.Name}")) result.Add(ninjaItem);
                else if (tradeItem is { Text: not null, Name: null } && ninjaItem.Stash?.Name == tradeItem.Text) result.Add(ninjaItem);
                else if (baseItem != null && ninjaItem.Stash?.Name != null && (ninjaItem.Stash.Name == baseItem.Name || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "Al-Hezmin Map (Tier 16)") || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "Baran Map (Tier 16)") || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "Drox Map (Tier 16)") || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "Veritania Map (Tier 16)") || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "The Constrictor Map (Tier 16)") || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "The Enslaver Map (Tier 16)") || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "The Eradicator Map (Tier 16)") || (baseItem.Name == "Map (Tier 16)" && ninjaItem.Stash.Name == "The Purifier Map (Tier 16)") || (baseItem.Name == "Blighted Map" && ninjaItem.Stash.Name.StartsWith("Blighted Map (Tier ")) || (baseItem.Name == "Blight-ravaged Map" && ninjaItem.Stash.Name == "Blight-ravaged Map (Tier 16)"))) result.Add(ninjaItem);
                else if (baseItem != null && ninjaItem.Stash?.BaseType != null && ninjaItem.Type != "UniqueJewel" && ninjaItem.Stash.BaseType == baseItem.Name && baseItem.Name is "Small Cluster Jewel" or "Medium Cluster Jewel" or "Large Cluster Jewel") result.Add(ninjaItem);
                else if (ninjaItem.Stash?.Name == (baseItem?.Name ?? tradeItem?.Type)) result.Add(ninjaItem);
            }
            return result.Count == 0 ? null : result;
        }
    }

    private record StaticItem(string Id, string? Text, string? Image);

    private async Task<List<StaticItem>> GetStaticDictionary(GameType game, IGameLanguage language)
    {
        var raw = await dataProvider.Read<RawTradeResult<List<RawTradeStaticItemCategory>>>(game, DataType.RawTradeStatic, language);
        var result = new List<StaticItem>();
        foreach (var category in raw.Result)
            foreach (var entry in category.Entries)
            {
                if (entry.Id == null! || entry.Text == null || entry.Id == "sep") continue;
                var image = string.IsNullOrEmpty(entry.Image) ? null : $"https://web.poecdn.com{entry.Image}";
                result.Add(new StaticItem(entry.Id, entry.Text, image));
            }
        return result;
    }

    private async Task<List<TradeItemDefinition>> GetTradeItems(GameType game, IGameLanguage language)
    {
        var itemsResult = await dataProvider.Read<RawTradeResult<List<RawTradeItemCategory>>>(game, DataType.RawTradeItems, language);
        var staticItems = await GetStaticDictionary(game, language);
        StaticItem? GetStatic(string? name, string? type)
        {
            var data = !string.IsNullOrEmpty(name) ? staticItems.FirstOrDefault(x => x.Text == name) : null;
            data ??= !string.IsNullOrEmpty(type) ? staticItems.FirstOrDefault(x => x.Text == type) : null;
            return data;
        }
        var result = new List<TradeItemDefinition>();
        foreach (var category in itemsResult.Result)
            foreach (var entry in category.Entries)
            {
                var staticItem = GetStatic(entry.Name, entry.Type);
                var text = staticItem?.Text ?? entry.Text;
                if (text == entry.Name || text == entry.Type) text = null;
                if (entry.Discriminator == "legacy") continue;
                result.Add(new TradeItemDefinition() { Id = staticItem?.Id, Image = staticItem?.Image, Name = entry.Name, Type = entry.Type, Text = text, Category = category.Id, Discriminator = entry.Discriminator });
            }
        return result;
    }

    private async Task<List<BaseItemDefinition>> GetBaseItems(GameType game, IGameLanguage language)
    {
        var lang = GetLanguageName(language.Code);
        var prefix = game == GameType.PathOfExile1 ? "poe1" : "poe2";

        var query = $"query {{ {prefix}_baseItemTypes(lang: \"{lang}\") {{ _index id name itemClassesKey itemClass dropLevel }} }}";
        var result = await graphQlClient.QueryAsync<BaseItemTypesResponse>(query);
        var graphItems = game == GameType.PathOfExile1 ? result?.Poe1BaseItemTypes : result?.Poe2BaseItemTypes;
        if (graphItems == null || graphItems.Count == 0)
        {
            logger.LogWarning("[ItemDefinitionBuilder] No base items from GraphQL for {Game}/{Lang}", game, language.Code);
            return new List<BaseItemDefinition>();
        }

        var armourQuery = $"query {{ {prefix}_armourTypes(lang: \"{lang}\") {{ baseItemTypesKey armourMin armourMax evasionMin evasionMax energyShieldMin energyShieldMax wardMin wardMax }} }}";
        var armourResult = await graphQlClient.QueryAsync<ArmourTypesResponse>(armourQuery);
        var armourItems = game == GameType.PathOfExile1 ? armourResult?.Poe1ArmourTypes : armourResult?.Poe2ArmourTypes;
        var armourLookup = armourItems?.ToDictionary(a => a.BaseItemTypesKey, a => a) ?? new Dictionary<int, GraphQlArmourType>();

        var baseItemDefinitions = new List<BaseItemDefinition>();
        foreach (var item in graphItems)
        {
            if (string.IsNullOrEmpty(item.Name)) continue;
            var itemClassId = ResolveItemClassId(game, item);
            BaseItemProperties? properties = null;
            BaseItemRequirements? requirements = null;
            if (armourLookup.TryGetValue(item.Index, out var armour))
            {
                properties = new BaseItemProperties
                {
                    Armour = armour.ArmourMin.HasValue || armour.ArmourMax.HasValue ? new BaseItemPropertyValues { Min = armour.ArmourMin, Max = armour.ArmourMax } : null,
                    EnergyShield = armour.EnergyShieldMin.HasValue || armour.EnergyShieldMax.HasValue ? new BaseItemPropertyValues { Min = armour.EnergyShieldMin, Max = armour.EnergyShieldMax } : null,
                    Evasion = armour.EvasionMin.HasValue || armour.EvasionMax.HasValue ? new BaseItemPropertyValues { Min = armour.EvasionMin, Max = armour.EvasionMax } : null,
                    Ward = armour.WardMin.HasValue || armour.WardMax.HasValue ? new BaseItemPropertyValues { Min = armour.WardMin, Max = armour.WardMax } : null,
                };
                requirements = new BaseItemRequirements { Level = item.DropLevel ?? 0 };
            }
            else if (item.DropLevel.HasValue)
            {
                requirements = new BaseItemRequirements { Level = item.DropLevel.Value };
            }
            baseItemDefinitions.Add(new BaseItemDefinition() { Id = item.Id, Name = item.Name, ItemClassId = itemClassId, Requirements = requirements, Properties = properties });
        }
        return baseItemDefinitions;
    }

    private async Task<List<UniqueItemDefinition>> GetUniqueItems(GameType game, IGameLanguage language)
    {
        logger.LogDebug("[ItemDefinitionBuilder] Unique items not available from GraphQL API for {Game}/{Lang}.", game, language.Code);
        return new List<UniqueItemDefinition>();
    }

    private static string? ResolveItemClassId(GameType game, GraphQlBaseItem item)
    {
        if (game == GameType.PathOfExile1)
            return item.ItemClassesKey.HasValue ? item.ItemClassesKey.Value.ToString() : null;
        else
            return item.ItemClass.HasValue ? item.ItemClass.Value.ToString() : null;
    }

    private static string GetLanguageName(string code) => code switch
    {
        "en" => "English", "de" => "German", "es" => "Spanish", "fr" => "French",
        "ja" => "Japanese", "ko" => "Korean", "pt" => "Portuguese", "ru" => "Russian",
        "th" => "Thai", "zh" => "Traditional Chinese", _ => "English",
    };
}
