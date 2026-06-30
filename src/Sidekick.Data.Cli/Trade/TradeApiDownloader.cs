using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Data.Cli.Trade.Dtos;
using Sidekick.Data.Extensions;
using Sidekick.Data.Languages;
using Sidekick.Data.Trade;

namespace Sidekick.Data.Cli.Trade;

public class TradeApiDownloader(
    ILogger<TradeApiDownloader> logger,
    DbContextOptions<DataDbContext> dbContextOptions)
{
    private static HttpClient CreateHttpClient()
    {
        var http = new HttpClient();
        http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Sidekick.Data", "1.0"));
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
        return http;
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.Preserve,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
    };

    public async Task Download(GameType game, IGameLanguage language)
    {
        await DownloadLeagues(game, language);
        await DownloadStatic(game, language);
        await DownloadItems(game, language);
        await DownloadStats(game, language);
        await DownloadFilters(game, language);
    }

    private static string GetApiBase(IGameLanguage language, GameType game)
    {
        return game == GameType.PathOfExile2 ? language.Poe2TradeApiBaseUrl : language.PoeTradeApiBaseUrl;
    }

    private async Task DownloadLeagues(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/leagues";
        logger.LogDebug($"[TradeApiDownloader] GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeLeagueDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var added = 0;
        await using var db = new DataDbContext(dbContextOptions);

        db.TradeLeagues.RemoveRange(db.TradeLeagues.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var league in result.Result)
        {
            if (league.Id == null) continue;

            db.TradeLeagues.Add(new TradeLeague
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                Language = language.Code,
                Id = league.Id,
                Realm = league.Realm,
                Text = league.Text,
            });
            added++;
        }

        await db.SaveChangesAsync();
        logger.LogInformation(
            $"[TradeApiDownloader] Downloaded {added} trade leagues for {game}/{language.Code}");
    }

    private async Task DownloadItems(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/items";
        logger.LogDebug($"[TradeApiDownloader] GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeItemCategoryDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        await using var db = new DataDbContext(dbContextOptions);
        var added = 0;

        db.TradeItemCategories.RemoveRange(
            db.TradeItemCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            var categoryId = Guid.NewGuid();
            db.TradeItemCategories.Add(new TradeItemCategory
            {
                SidekickId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
                Label = category.Label,
            });

            foreach (var entry in category.Entries)
            {
                db.TradeItems.Add(new TradeItem
                {
                    SidekickId = Guid.NewGuid(),
                    Game = game,
                    Language = language.Code,
                    CategoryId = categoryId,
                    Name = entry.Name,
                    Type = entry.Type,
                    Text = entry.Text,
                    Discriminator = entry.Discriminator,
                    IsUnique = entry.Flags?.Unique ?? false,
                    StaticItem = await GetTradeStaticItem(db, entry.Text, entry.Type),
                });
                added++;
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation($"[TradeApiDownloader] Downloaded {added} trade items for {game}/{language.Code}");

        return;

        async Task<TradeStaticItem?> GetTradeStaticItem(DataDbContext dbContext, string? name, string? type)
        {
            var staticItem = await dbContext.TradeStaticItems
                .Where(x => x.Game == game && x.Language == language.Code)
                .FirstOrDefaultAsync(x => x.Text == name);
            staticItem ??= await dbContext.TradeStaticItems
                .Where(x => x.Game == game && x.Language == language.Code)
                .FirstOrDefaultAsync(x => x.Text == type);
            return staticItem;
        }
    }

    private async Task DownloadStats(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/stats";
        logger.LogDebug($"[TradeApiDownloader] GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeStatCategoryDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var statsAdded = 0;

        await using var db = new DataDbContext(dbContextOptions);

        db.TradeStatCategories.RemoveRange(
            db.TradeStatCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            var categoryId = Guid.NewGuid();
            db.TradeStatCategories.Add(new TradeStatCategory
            {
                SidekickId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
                Label = category.Label,
            });

            foreach (var entry in category.Entries)
            {
                if (entry.Option?.Options != null)
                {
                    foreach (var option in entry.Option.Options)
                    {
                        db.TradeStats.Add(new TradeStat
                        {
                            Id = $"{entry.Id}#{option.Id}",
                            Text = entry.Text.RemoveSquareBrackets(),
                            OptionText = option.Text.RemoveSquareBrackets(),
                            SidekickId = Guid.NewGuid(),
                            Game = game,
                            Language = language.Code,
                            CategoryId = categoryId,
                            Type = entry.Type,
                        });
                        statsAdded++;
                    }

                    continue;
                }

                db.TradeStats.Add(new TradeStat
                {
                    SidekickId = Guid.NewGuid(),
                    Game = game,
                    Language = language.Code,
                    Id = entry.Id,
                    CategoryId = categoryId,
                    Text = entry.Text.RemoveSquareBrackets(),
                    OptionText = null,
                    Type = entry.Type,
                });
                statsAdded++;
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation($"[TradeApiDownloader] Downloaded {statsAdded} trade stats for {game}/{language.Code}");
    }

    private async Task DownloadStatic(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/static";
        logger.LogDebug($"[TradeApiDownloader] GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result =
            JsonSerializer.Deserialize<TradeApiResponse<TradeStaticItemCategoryDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var added = 0;

        await using var db = new DataDbContext(dbContextOptions);

        db.TradeStaticItemCategories.RemoveRange(
            db.TradeStaticItemCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            var categoryId = Guid.NewGuid();
            db.TradeStaticItemCategories.Add(new TradeStaticItemCategory
            {
                SidekickId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
                Label = category.Label,
            });

            foreach (var entry in category.Entries)
            {
                db.TradeStaticItems.Add(new TradeStaticItem
                {
                    SidekickId = Guid.NewGuid(),
                    Game = game,
                    Language = language.Code,
                    Id = entry.Id,
                    CategoryId = categoryId,
                    Text = entry.Text,
                    Image = entry.Image,
                });
                added++;
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation($"[TradeApiDownloader] Downloaded {added} trade static items for {game}/{language.Code}");
    }

    private async Task DownloadFilters(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/filters";
        logger.LogDebug($"[TradeApiDownloader] GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeFilterGroupDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        int filtersAdded = 0, optionsAdded = 0;

        await using var db = new DataDbContext(dbContextOptions);

        db.TradeFilterCategories.RemoveRange(
            db.TradeFilterCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            if (category.Id == null) continue;

            var categoryId = Guid.NewGuid();
            db.TradeFilterCategories.Add(new TradeFilterCategory
            {
                SidekickId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
            });

            foreach (var filter in category.Filters)
            {
                var id = Guid.NewGuid();

                db.TradeFilters.Add(new TradeFilter()
                {
                    SidekickId = id,
                    Game = game,
                    Language = language.Code,
                    CategoryId = categoryId,
                    Id = filter.Id,
                    Text = filter.Text,
                    Hidden = filter.Hidden,
                    FullSpan = filter.FullSpan,
                    HalfSpan = filter.HalfSpan,
                    MinMax = filter.MinMax,
                    Sockets = filter.Sockets,
                    Tip = filter.Tip,
                });
                filtersAdded++;

                // Add options if present
                if (filter.Option?.Options != null)
                {
                    foreach (var option in filter.Option.Options)
                    {
                        db.TradeFilterOptions.Add(new TradeFilterOption()
                        {
                            SidekickId = Guid.NewGuid(),
                            FilterId = id,
                            Id = option.Id ?? "",
                            Text = option.Text,
                        });
                        optionsAdded++;
                    }
                }
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation($"[TradeApiDownloader] Downloaded {filtersAdded} trade filters and {optionsAdded} options for {game}/{language.Code}");
    }
}