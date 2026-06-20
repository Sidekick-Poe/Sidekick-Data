using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Common.Enums;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.GraphQl.Models;
using Sidekick.Data.ItemClasses;
using Sidekick.Data.ItemDefinitions;
using Sidekick.Data.Languages;

namespace Sidekick.Data.Cli.ItemClasses;

public class ItemClassBuilder(
    ILogger<ItemClassBuilder> logger,
    IOptions<SidekickConfiguration> configuration,
    DataProvider dataProvider,
    GraphQlClient graphQlClient)
{
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
            logger.LogError(ex, "Failed to build item class data.");
        }
    }

    private async Task BuildForGame(GameType game, IGameLanguage language)
    {
        var lang = GraphQlClient.GetLanguageName(language.Code);
        var prefix = game == GameType.PathOfExile1 ? "poe1" : "poe2";
        var query = $"query {{ {prefix}_itemClasses(lang: \"{lang}\") {{ id name }} }}";
        var result = await graphQlClient.QueryAsync<ItemClassesResponse>(query);
        var items = game == GameType.PathOfExile1 ? result?.Poe1ItemClasses : result?.Poe2ItemClasses;
        if (items == null || items.Count == 0)
        {
            logger.LogWarning("[ItemClassBuilder] No item classes from GraphQL for {Game}/{Lang}", game, language.Code);
            return;
        }
        var defs = new List<ItemClassDefinition>();
        foreach (var item in items)
        {
            if (string.IsNullOrEmpty(item.Id)) continue;
            defs.Add(new ItemClassDefinition()
            {
                Id = item.Id,
                Name = item.Name,
                Type = EnumExtensions.FindValue<ItemClass>(ic => ic.FindAttributes<ItemClassGameId>().Any(attr => attr.Id == item.Id && attr.Game == game)),
            });
        }
        await dataProvider.Write(game, DataType.ItemClasses, language, defs);
    }

}
