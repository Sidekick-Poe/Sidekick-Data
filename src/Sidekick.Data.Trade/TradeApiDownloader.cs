using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Data.Languages;
using Sidekick.Data.Trade.Models;

namespace Sidekick.Data.Trade;

public class TradeApiDownloader
{
    private readonly ILogger<TradeApiDownloader> logger;
    private readonly IOptions<SidekickConfiguration> configuration;
    private readonly string dbPath;

    public TradeApiDownloader(
        ILogger<TradeApiDownloader> logger,
        IOptions<SidekickConfiguration> configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.dbPath = Path.Combine("data", "trade.db");
    }

    public async Task Download(IGameLanguage language)
    {
        try
        {
            foreach (var game in new[] { GameType.PathOfExile1, GameType.PathOfExile2 })
            {
                // Use a fresh DbContext per game to avoid entity tracking conflicts
                await using var gameDb = CreateDbContext();
                await DownloadItems(gameDb, game, language);
                await DownloadStats(gameDb, game, language);
                await DownloadStatic(gameDb, game, language);
                await DownloadFilters(gameDb, game, language);
                await gameDb.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (configuration.Value.ApplicationType == SidekickApplicationType.DataBuilder ||
                configuration.Value.ApplicationType == SidekickApplicationType.Test)
                throw;
            logger.LogError(ex, $"Failed to download trade data for {language.Code}.");
        }
    }

    private TradeDbContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<TradeDbContext>();
        builder.UseSqlite($"Data Source={dbPath}");
        return new TradeDbContext(builder.Options);
    }

    private static string GetApiBase(IGameLanguage language, GameType game)
    {
        return game == GameType.PathOfExile2 ? language.Poe2TradeApiBaseUrl : language.PoeTradeApiBaseUrl;
    }

    private async Task DownloadItems(TradeDbContext db, GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/items";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<RawTradeItemsResponse>(json, JsonOptions);
        if (result?.Result == null) return;

        var gameNum = (int)game;
        var seenIds = new HashSet<string>();
        int added = 0, skipped = 0;

        foreach (var category in result.Result)
        {
            if (category.Id == null) continue;
            db.ItemCategories.Add(new TradeItemCategory
            {
                Game = gameNum,
                Language = language.Code,
                Id = category.Id,
                Label = category.Label,
            });

            foreach (var entry in category.Entries)
            {
                if (entry.Name == null && entry.Type == null) continue;
                var id = (entry.Type ?? entry.Name ?? "") + "|" + category.Id + "|" + gameNum;
                if (!seenIds.Add(id))
                {
                    skipped++;
                    continue;
                }

                db.Items.Add(new TradeItem
                {
                    Game = gameNum,
                    Language = language.Code,
                    Id = id,
                    CategoryId = category.Id,
                    Name = entry.Name,
                    Type = entry.Type,
                    Text = entry.Text,
                    Discriminator = entry.Discriminator,
                    IsUnique = entry.Flags?.Unique ?? false
                });
                added++;
            }
        }

        logger.LogInformation(
            $"Downloaded {added} trade items ({skipped} duplicates skipped) for {game}/{language.Code}");
    }

    private async Task DownloadStats(TradeDbContext db, GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/stats";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<RawTradeStatsResponse>(json, JsonOptions);
        if (result?.Result == null) return;

        int gameNum = (int)game;
        var seenIds = new HashSet<string>();
        int added = 0, skipped = 0;

        foreach (var category in result.Result)
        {
            if (category.Id == null) continue;
            db.StatCategories.Add(new TradeStatCategory
            {
                Game = gameNum, Language = language.Code, Id = category.Id, Label = category.Label
            });
            foreach (var entry in category.Entries)
            {
                var id = entry.Id + "|" + gameNum;
                if (!seenIds.Add(id))
                {
                    skipped++;
                    continue;
                }

                db.Stats.Add(new TradeStat
                {
                    Game = gameNum, Language = language.Code,
                    Id = entry.Id, CategoryId = category.Id,
                    Text = entry.Text, Type = entry.Type,
                    OptionId = entry.Options?.Options?.FirstOrDefault().Id,
                    OptionText = entry.Options?.Options?.FirstOrDefault().Text
                });
                added++;
            }
        }

        logger.LogInformation(
            $"Downloaded {added} trade stats ({skipped} duplicates skipped) for {game}/{language.Code}");
    }

    private async Task DownloadStatic(TradeDbContext db, GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/static";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<RawTradeStaticResponse>(json, JsonOptions);
        if (result?.Result == null) return;

        int gameNum = (int)game;
        var seenIds = new HashSet<string>();
        int added = 0, skipped = 0;

        foreach (var category in result.Result)
        {
            if (category.Id == null) continue;
            db.StaticItemCategories.Add(new TradeStaticItemCategory
            {
                Game = gameNum, Language = language.Code, Id = category.Id, Label = category.Label
            });
            foreach (var entry in category.Entries)
            {
                if (entry.Id == null) continue;
                var id = entry.Id + "|" + gameNum;
                if (!seenIds.Add(id))
                {
                    skipped++;
                    continue;
                }

                db.StaticItems.Add(new TradeStaticItem
                {
                    Game = gameNum, Language = language.Code,
                    Id = entry.Id, CategoryId = category.Id,
                    Text = entry.Text, Image = entry.Image
                });
                added++;
            }
        }

        logger.LogInformation(
            $"Downloaded {added} trade static items ({skipped} duplicates skipped) for {game}/{language.Code}");
    }

    private async Task DownloadFilters(TradeDbContext db, GameType game, IGameLanguage language)
    {
        var url = GetApiBase(language, game) + "data/filters";
        logger.LogInformation($"GET {url}");

        using var http = CreateHttpClient();
        var json = await http.GetStringAsync(url);
        var result = JsonSerializer.Deserialize<RawTradeFiltersResponse>(json, JsonOptions);
        if (result?.Result == null) return;

        int gameNum = (int)game;
        foreach (var filter in result.Result)
        {
            if (filter.Id == null) continue;
            db.Filters.Add(new Models.TradeFilter()
            {
                Game = gameNum, Language = language.Code,
                Id = filter.Id, Text = filter.Text, Type = filter.Type
            });
        }

        logger.LogInformation($"Downloaded {result.Result.Count} trade filters for {game}/{language.Code}");
    }

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
    };

    // Raw JSON response models
    private sealed record RawTradeItemsResponse(List<RawTradeItemCategory> Result);

    private sealed record RawTradeItemCategory(string? Id, string? Label, List<RawTradeItem> Entries);

    private sealed record RawTradeItem(
        string? Name,
        string? Type,
        string? Text,
        [property: JsonPropertyName("disc")] string? Discriminator,
        RawTradeItemFlags? Flags);

    private sealed record RawTradeItemFlags(bool Unique);

    private sealed record RawTradeStatsResponse(List<RawTradeStatCategory> Result);

    private sealed record RawTradeStatCategory(string? Id, string? Label, List<RawTradeStatEntry> Entries);

    private sealed record RawTradeStatEntry(
        string Id,
        string Text,
        string Type,
        [property: JsonPropertyName("option")] RawTradeStatOption? Options);

    private sealed record RawTradeStatOption(List<RawTradeStatOptionValue> Options);

    private sealed record RawTradeStatOptionValue(int Id, string Text);

    private sealed record RawTradeStaticResponse(List<RawTradeStaticCategory> Result);

    private sealed record RawTradeStaticCategory(string? Id, string? Label, List<RawTradeStaticEntry> Entries);

    private sealed record RawTradeStaticEntry(string Id, string? Text, string? Image);

    private sealed record RawTradeFiltersResponse(List<RawTradeFilter> Result);

    private sealed record RawTradeFilter(string Id, string? Text, string? Type);
}