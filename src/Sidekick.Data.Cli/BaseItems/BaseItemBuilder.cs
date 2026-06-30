using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Data.BaseItems;
using Sidekick.Data.Cli.BaseItems.Responses;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Languages;

namespace Sidekick.Data.Cli.BaseItems;

public class BaseItemBuilder(
    ILogger<BaseItemBuilder> logger,
    DbContextOptions<DataDbContext> dbContextOptions,
    GraphQlClient graphQlClient)
{
    public async Task Build(GameType game, IGameLanguage language)
    {
        await using var dbContext = new DataDbContext(dbContextOptions);
        dbContext.BaseItems.RemoveRange(dbContext.BaseItems.Where(x => x.Game == game && x.Language == language.Code));
        await dbContext.SaveChangesAsync();

        // Fetch from GraphQL
        var lang = GraphQlClient.GetLanguageName(language.Code);

        var query = GraphQlBaseItem.GetQuery(game, lang);
        var result = await graphQlClient.QueryAsync<GraphQlBaseItemTypesResponse>(query);
        var graphItems = result?.Poe1 ?? result?.Poe2;
        if (graphItems == null || graphItems.Count == 0)
        {
            throw new Exception($"[BaseItemBuilder] No base items from GraphQL for {game}/{language.Code}");
        }

        var armourQuery = IGraphQlArmourType.GetQuery(game, lang);
        var armourResult = await graphQlClient.QueryAsync<GraphQlArmourTypesResponse>(armourQuery);
        var armourItems = game == GameType.PathOfExile1
            ? armourResult?.Poe1?.Cast<IGraphQlArmourType>().ToList() ?? []
            : armourResult?.Poe2?.Cast<IGraphQlArmourType>().ToList() ?? [];
        var armourLookup = armourItems?.ToDictionary(a => a.BaseItem.Id, a => a) ?? new();
        if (armourItems == null || armourItems.Count == 0)
        {
            throw new Exception($"[BaseItemBuilder] No armour types from GraphQL for {game}/{language.Code}");
        }

        var requirementQuery = IGraphQlAttributeRequirement.GetQuery(game, lang);
        var requirementResult = await graphQlClient.QueryAsync<GraphQlAttributeRequirementsResponse>(requirementQuery);
        var requirementItems = game == GameType.PathOfExile1
            ? requirementResult?.Poe1?.Cast<IGraphQlAttributeRequirement>().ToList() ?? []
            : requirementResult?.Poe2?.Cast<IGraphQlAttributeRequirement>().ToList() ?? [];
        var requirementLookup = requirementItems?.ToDictionary(a => a.BaseItemId, a => a) ?? new();
        if (requirementItems == null || requirementItems.Count == 0)
        {
            throw new Exception($"[BaseItemBuilder] No requirement types from GraphQL for {game}/{language.Code}");
        }

        var itemClasses = await dbContext.ItemClasses
            .Where(x => x.Game == game && x.Language == language.Code)
            .ToListAsync();

        var added = 0;

        foreach (var item in graphItems)
        {
            if (string.IsNullOrEmpty(item.Name)) continue;

            var armour = armourLookup.GetValueOrDefault(item.Id);
            var requirement = requirementLookup.GetValueOrDefault(item.Id);

            var entity = new BaseItem
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                Language = language.Code,
                Id = item.Id,
                Name = item.Name,
                DropLevel = item.DropLevel ?? 0,
                ItemClassId = itemClasses.FirstOrDefault(x => x.Id == item.ItemClass?.Id)?.SidekickId,
                ItemVisualIdentityId = item.ItemVisualIdentity?.Id,

                ArmourMin = armour?.ArmourMin ?? 0,
                ArmourMax = armour?.ArmourMax ?? 0,
                EnergyShieldMin = armour?.EnergyShieldMin ?? 0,
                EnergyShieldMax = armour?.EnergyShieldMax ?? 0,
                EvasionMin = armour?.EvasionMin ?? 0,
                EvasionMax = armour?.EvasionMax ?? 0,
                WardMin = armour?.WardMin ?? 0,
                WardMax = armour?.WardMax ?? 0,

                RequiresDexterity = requirement?.RequiresDexterity ?? 0,
                RequiresIntelligence = requirement?.RequiresIntelligence ?? 0,
                RequiresStrength = requirement?.RequiresStrength ?? 0,
            };

            dbContext.BaseItems.Add(entity);
            added++;
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation("[BaseItemBuilder] Created {Count} base items for {Game}/{Lang}", added, game,
            language.Code);
    }
}