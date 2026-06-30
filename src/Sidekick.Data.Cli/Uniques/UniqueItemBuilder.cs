using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.Uniques.Responses;
using Sidekick.Data.Languages;
using Sidekick.Data.Uniques;

namespace Sidekick.Data.Cli.Uniques;

public class UniqueItemBuilder(
    ILogger<UniqueItemBuilder> logger,
    DbContextOptions<DataDbContext> dbContextOptions,
    GraphQlClient graphQlClient)
{
    public async Task Build(GameType game, IGameLanguage language)
    {
        var lang = GraphQlClient.GetLanguageName(language.Code);

        var query = GraphQlUniqueItem.GetQuery(game, lang);
        var result = await graphQlClient.QueryAsync<GraphQlUniqueItemsResponse>(query);
        var items = result?.Poe1 ?? result?.Poe2;
        if (items == null || items.Count == 0)
        {
            throw new Exception($"[UniqueItemBuilder] No unique items from GraphQL for {game}/{language.Code}");
        }

        await using var dbContext = new DataDbContext(dbContextOptions);
        dbContext.UniqueItems.RemoveRange(
            dbContext.UniqueItems.Where(x => x.Game == game && x.Language == language.Code));
        await dbContext.SaveChangesAsync();
        var added = 0;

        var baseItems = await dbContext.BaseItems
            .Where(x => x.Game == game && x.Language == language.Code)
            .ToListAsync();

        foreach (var item in items)
        {
            if (string.IsNullOrEmpty(item.ItemVisualIdentity?.Id)) continue;
            if (string.IsNullOrEmpty(item.Words?.Text)) continue;

            dbContext.UniqueItems.Add(new UniqueItem()
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                Language = language.Code,
                Id = item.ItemVisualIdentity.Id,
                Name = item.Words.Text,
                Image = item.ItemVisualIdentity.Image?.Replace(".dds", ".png", StringComparison.InvariantCultureIgnoreCase),
                BaseItemId = baseItems.FirstOrDefault(x => x.ItemVisualIdentityId == item.ItemVisualIdentity.Id)?.SidekickId ?? null,
            });
            added++;
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation($"Built {added} unique items for {game}/{language.Code}");
    }
}