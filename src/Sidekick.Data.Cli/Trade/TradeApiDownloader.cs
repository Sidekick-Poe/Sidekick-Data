using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Data.Cli.Trade.Dtos;
using Sidekick.Data.Languages;
using Sidekick.Data.Leagues;
using Sidekick.Data.Trade;
using Sidekick.Data.Trade.Models;

namespace Sidekick.Data.Cli.Trade;

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
        var url = GetApiBase(language, game) + "data/leagues";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeLeagueDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        var added = 0;
        await using var db = new TradeDbContext(dbContextOptions);

        db.Leagues.RemoveRange(db.Leagues.Where(x => x.Game == game && x.Language == language.Code));
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

        int added = 0;

        await using var db = new TradeDbContext(dbContextOptions);

        db.ItemCategories.RemoveRange(db.ItemCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            var categoryId = Guid.NewGuid();
            db.ItemCategories.Add(new TradeItemCategory
            {
                UniqueId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
                Label = category.Label,
            });

            foreach (var entry in category.Entries)
            {
                db.Items.Add(new TradeItem
                {
                    UniqueId = Guid.NewGuid(),
                    Game = game,
                    Language = language.Code,
                    CategoryId = categoryId,
                    Name = entry.Name,
                    Type = entry.Type,
                    Text = entry.Text,
                    Discriminator = entry.Discriminator,
                });
                added++;
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation($"Downloaded {added} trade items for {game}/{language.Code}");
    }

    private async Task DownloadStats(GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/stats";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<TradeApiResponse<TradeStatCategoryDto>>(json, JsonOptions);
        if (result?.Result == null) return;

        int statsAdded = 0, optionsAdded = 0;

        await using var db = new TradeDbContext(dbContextOptions);

        db.StatCategories.RemoveRange(db.StatCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            var categoryId = Guid.NewGuid();
            db.StatCategories.Add(new TradeStatCategory
            {
                UniqueId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
                Label = category.Label,
            });

            foreach (var entry in category.Entries)
            {
                var id = Guid.NewGuid();

                db.Stats.Add(new TradeStat
                {
                    UniqueId = id,
                    Game = game,
                    Language = language.Code,
                    Id = entry.Id,
                    CategoryId = categoryId,
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
                            TradeStatUniqueId = id,
                            Id = option.Id,
                            Text = option.Text,
                        });
                        optionsAdded++;
                    }
                }
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation(
            $"Downloaded {statsAdded} trade stats and {optionsAdded} options for {game}/{language.Code}");
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

        var added = 0;

        await using var db = new TradeDbContext(dbContextOptions);

        db.StaticItemCategories.RemoveRange(db.StaticItemCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            var categoryId = Guid.NewGuid();
            db.StaticItemCategories.Add(new TradeStaticItemCategory
            {
                UniqueId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
                Label = category.Label,
            });

            foreach (var entry in category.Entries)
            {
                db.StaticItems.Add(new TradeStaticItem
                {
                    UniqueId = Guid.NewGuid(),
                    Game = game,
                    Language = language.Code,
                    Id = entry.Id,
                    CategoryId = categoryId,
                    Text = entry.Text,
                    Image = entry.Image
                });
                added++;
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation(
            $"Downloaded {added} trade static items for {game}/{language.Code}");
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

        db.FilterCategories.RemoveRange(db.FilterCategories.Where(x => x.Game == game && x.Language == language.Code));
        await db.SaveChangesAsync();

        foreach (var category in result.Result)
        {
            if (category.Id == null) continue;

            var categoryId = Guid.NewGuid();
            db.FilterCategories.Add(new Data.Trade.Models.TradeFilterCategory
            {
                UniqueId = categoryId,
                Game = game,
                Language = language.Code,
                Id = category.Id,
            });

            foreach (var filter in category.Filters)
            {
                var id = Guid.NewGuid();

                db.Filters.Add(new Data.Trade.Models.TradeFilter()
                {
                    UniqueId = id,
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
                        db.FilterOptions.Add(new Data.Trade.Models.TradeFilterOption()
                        {
                            FilterUniqueId = id,
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