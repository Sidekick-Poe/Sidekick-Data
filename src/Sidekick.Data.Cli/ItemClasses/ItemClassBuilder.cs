using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Common.Enums;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.ItemClasses.Responses;
using Sidekick.Data.ItemClasses;
using Sidekick.Data.Languages;

namespace Sidekick.Data.Cli.ItemClasses;

public class ItemClassBuilder(
    ILogger<ItemClassBuilder> logger,
    DbContextOptions<DataDbContext> dbContextOptions,
    GraphQlClient graphQlClient)
{
    public async Task Build(GameType game, IGameLanguage language)
    {
        var lang = GraphQlClient.GetLanguageName(language.Code);

        var query = GraphQlItemClass.GetQuery(game, lang);
        var result = await graphQlClient.QueryAsync<GraphQlItemClassesResponse>(query);
        var items = result?.Poe1 ?? result?.Poe2;
        if (items == null || items.Count == 0)
        {
            throw new Exception($"[ItemClassBuilder] No item classes from GraphQL for {game}/{language.Code}");
        }

        await using var dbContext = new DataDbContext(dbContextOptions);
        dbContext.ItemClasses.RemoveRange(
            dbContext.ItemClasses.Where(x => x.Game == game && x.Language == language.Code));
        await dbContext.SaveChangesAsync();
        var added = 0;

        foreach (var item in items)
        {
            if (string.IsNullOrEmpty(item.Id)) continue;
            var type = EnumExtensions.FindValue<ItemClass>(ic =>
                ic.FindAttributes<ItemClassGameId>().Any(attr => attr.Id == item.Id && attr.Game == game));
            dbContext.ItemClasses.Add(new ItemClassEntity
            {
                Game = game,
                Language = language.Code,
                Id = item.Id,
                Name = item.Name,
                Type = type,
            });
            added++;
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation("[ItemClassBuilder] Created {Added} item classes for {GameType}/{LanguageCode}", added, game, language.Code);
    }
}