using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Data.Languages;
using Sidekick.Data.Leagues;
using Sidekick.Data.Trade.Dtos;
using Sidekick.Data.Trade.Models;

namespace Sidekick.Data.Trade;

public class TradeApiDownloader(
    ILogger<TradeApiDownloader> logger,
    IOptions<SidekickConfiguration> configuration,
    DbContextOptions<TradeDbContext> dbContextOptions)
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
        try
        {
            await DownloadLeagues(game, language);
            await DownloadItems(game, language);
            await DownloadStats(game, language);
            await DownloadStatic(game, language);
            await DownloadFilters(game, language);
        }
        catch (Exception ex)
        {
            if (configuration.Value.ApplicationType == SidekickApplicationType.DataBuilder ||
                configuration.Value.ApplicationType == SidekickApplicationType.Test)
                throw;
            logger.LogError(ex, $"Failed to download trade data for {language.Code}.");
        }
    }

    private static string GetApiBase(IGameLanguage language, GameType game)
    {
        return game == GameType.PathOfExile2 ? language.Poe2TradeApiBaseUrl : language.PoeTradeApiBaseUrl;
    }

    private async Task DownloadLeagues(GameType game, IGameLanguage language)
    {
        if (language.Code != "en") return;

        var url = GetApiBase(language, game) + "data/leagues";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeLeagueDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var added = 0;
        await using var db = new TradeDbContext(dbContextOptions);

        db.Leagues.RemoveRange(db.Leagues);
        await db.SaveChangesAsync();

        foreach (var league in result.Result)
        {
            if (league.Id == null) continue;
            if (league.Realm != LeagueRealm.PC && league.Realm != LeagueRealm.Poe2) continue;

            db.Leagues.Add(new TradeLeague
            {
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
            $"Downloaded {added} trade leagues for {game}/{language.Code}");
    }

    private async Task DownloadItems(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/items";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeItemCategoryDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var seenIds = new HashSet<string>();
        int added = 0, skipped = 0;

        await using var db = new TradeDbContext(dbContextOptions);

        db.Items.RemoveRange(db.Items.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            foreach (var entry in category.Entries)
            {
                if (entry.Name == null && entry.Type == null) continue;
                var id = $"{entry.Type}_{entry.Name}_{entry.Text}_{entry.Discriminator}";
                if (!seenIds.Add(id))
                {
                    skipped++;
                    continue;
                }

                db.Items.Add(new TradeItem
                {
                    Game = game,
                    Language = language.Code,
                    Id = id,
                    CategoryId = category.Id,
                    Name = entry.Name,
                    Type = entry.Type,
                    Text = entry.Text,
                    Discriminator = entry.Discriminator,
                });
                added++;
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation(
            $"Downloaded {added} trade items ({skipped} duplicates skipped) for {game}/{language.Code}");
    }

    private async Task DownloadStats(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/stats";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeStatCategoryDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var seenStatIds = new HashSet<string>();
        int statsAdded = 0, statsSkipped = 0, optionsAdded = 0;

        await using var db = new TradeDbContext(dbContextOptions);

        db.StatOptions.RemoveRange(db.StatOptions.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        db.Stats.RemoveRange(db.Stats.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            foreach (var entry in category.Entries)
            {
                var statId = entry.Id;
                if (!seenStatIds.Add(statId))
                {
                    statsSkipped++;
                    continue;
                }

                db.Stats.Add(new TradeStat
                {
                    Game = game,
                    Language = language.Code,
                    Id = entry.Id,
                    CategoryId = category.Id,
                    Text = entry.Text,
                    Type = entry.Type,
                });
                statsAdded++;

                if (entry.Option?.Options != null)
                {
                    foreach (var option in entry.Option.Options)
                    {
                        db.StatOptions.Add(new TradeStatOption
                        {
                            Game = game,
                            Language = language.Code,
                            StatId = entry.Id,
                            Id = option.Id,
                            Text = option.Text
                        });
                        optionsAdded++;
                    }
                }
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation(
            $"Downloaded {statsAdded} trade stats ({statsSkipped} duplicates skipped) and {optionsAdded} options for {game}/{language.Code}");
    }

    private async Task DownloadStatic(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/static";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result =
            JsonSerializer.Deserialize<TradeApiResponse<TradeStaticItemCategoryDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var seenIds = new HashSet<string>();
        int added = 0, skipped = 0;

        await using var db = new TradeDbContext(dbContextOptions);

        db.StaticItems.RemoveRange(db.StaticItems.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            foreach (var entry in category.Entries)
            {
                var id = entry.Id;
                if (!seenIds.Add(id))
                {
                    skipped++;
                    continue;
                }

                db.StaticItems.Add(new TradeStaticItem
                {
                    Game = game,
                    Language = language.Code,
                    Id = entry.Id,
                    CategoryId = category.Id,
                    Text = entry.Text,
                    Image = entry.Image
                });
                added++;
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation(
            $"Downloaded {added} trade static items ({skipped} duplicates skipped) for {game}/{language.Code}");
    }

    private async Task DownloadFilters(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/filters";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeFilterGroupDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        int filtersAdded = 0, optionsAdded = 0;

        await using var db = new TradeDbContext(dbContextOptions);

        db.FilterOptions.RemoveRange(db.FilterOptions.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        db.Filters.RemoveRange(db.Filters.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            if (category.Id == null) continue;

            foreach (var filter in category.Filters)
            {
                db.Filters.Add(new Models.TradeFilter()
                {
                    Game = game,
                    Language = language.Code,
                    CategoryId = category.Id,
                    Id = filter.Id,
                    Text = filter.Text,
                    Hidden = filter.Hidden,
                    FullSpan = filter.FullSpan,
                    HalfSpan = filter.HalfSpan,
                    MinMax = filter.MinMax,
                    Sockets = filter.Sockets,
                    Tip = filter.Tip
                });
                filtersAdded++;

                // Add options if present
                if (filter.Option?.Options != null)
                {
                    foreach (var option in filter.Option.Options)
                    {
                        db.FilterOptions.Add(new Models.TradeFilterOption()
                        {
                            Game = game,
                            Language = language.Code,
                            FilterGroupId = category.Id,
                            FilterId = filter.Id,
                            Id = option.Id ?? "",
                            Text = option.Text
                        });
                        optionsAdded++;
                    }
                }
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation(
            $"Downloaded {filtersAdded} trade filters and {optionsAdded} options for {game}/{language.Code}");
    }
}