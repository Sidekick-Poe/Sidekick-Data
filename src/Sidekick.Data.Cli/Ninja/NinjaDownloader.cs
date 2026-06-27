using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Common.Enums;
using Sidekick.Data.Cli.Ninja.Dtos;
using Sidekick.Data.Languages;
using Sidekick.Data.Ninja;
using Sidekick.Data.Ninja.Models;
using Sidekick.Data.Trade;

namespace Sidekick.Data.Cli.Ninja;

public class NinjaDownloader(
    ILogger<NinjaDownloader> logger,
    IOptions<SidekickConfiguration> configuration,
    DbContextOptions<NinjaDbContext> dbContextOptions,
    DbContextOptions<TradeDbContext> tradeDbContextOptions)
{
    private static readonly List<NinjaPage> Poe1Pages =
    [
        new("Currency", "currency", true, true),
        new("Fragment", "fragments", true, true),
        new("Wombgift", "wombgifts", false, true),
        new("Runegraft", "runegrafts", true, true),
        new("AllflameEmber", "allflame-embers", true, true),
        new("Tattoo", "tattoos", true, true),
        new("Omen", "omens", true, true),
        new("DjinnCoin", "djinn-coins", true, true),
        new("DivinationCard", "divination-cards", true, true),
        new("Artifact", "artifacts", true, true),
        new("Oil", "oils", true, true),
        new("Incubator", "incubators", false, true),
        new("UniqueWeapon", "unique-weapons", false, true),
        new("UniqueArmour", "unique-armours", false, true),
        new("UniqueAccessory", "unique-accessories", false, true),
        new("UniqueFlask", "unique-flasks", false, true),
        new("UniqueJewel", "unique-jewels", false, true),
        new("ForbiddenJewel", "forbidden-jewels", false, true),
        new("ShrineBelt", "shrine-belts", false, true),
        new("UniqueTincture", "unique-tinctures", false, true),
        new("UniqueRelic", "unique-relics", false, true),
        new("SkillGem", "skill-gems", false, true),
        new("ClusterJewel", "cluster-jewels", false, true),
        new("Map", "maps", false, true),
        new("BlightedMap", "blighted-maps", false, true),
        new("BlightRavagedMap", "blight-ravaged-maps", false, true),
        new("UniqueMap", "unique-maps", false, true),
        new("ValdoMap", "valdo-maps", false, true),
        new("DeliriumOrb", "delirium-orbs", true, true),
        new("Invitation", "invitations", false, true),
        new("Scarab", "scarabs", true, true),
        new("Astrolabe", "astrolabes", true, false),
        new("Memory", "memories", false, true),
        new("IncursionTemple", "temples", false, true),
        new("BaseType", "base-types", false, true),
        new("Fossil", "fossils", true, true),
        new("Resonator", "resonators", true, true),
        new("Beast", "beasts", false, true),
        new("Essence", "essences", true, true),
        new("Vial", "vials", false, true)
    ];

    private static readonly List<NinjaPage> Poe2Pages =
    [
        new("Currency", "currency", true, false),
        new("Fragments", "fragments", true, false),
        new("Abyss", "abyssal-bones", true, false),
        new("UncutGems", "uncut-gems", true, false),
        new("LineageSupportGems", "lineage-support-gems", true, false),
        new("Essences", "essences", true, false),
        new("Ultimatum", "soul-cores", true, false),
        new("Talismans", "talismans", true, false),
        new("Runes", "runes", true, false),
        new("Ritual", "omens", true, false),
        new("Expedition", "expedition", true, false),
        new("Delirium", "distilled-emotions", true, false),
        new("Breach", "breach-catalyst", true, false)
    ];

    private record NinjaPage(
        string Type,
        string Url,
        bool SupportsExchange,
        bool SupportsStash);

    public async Task Download(GameType game, IGameLanguage language)
    {
        if (language.Code != "en") return;

        await using var tradeDb = new TradeDbContext(tradeDbContextOptions);
        var league = await tradeDb.Leagues.FirstAsync(x => x.Game == game && x.Language == language.Code);

        try
        {
            await DownloadExchange(game, league.Id);
            await DownloadStash(game, league.Id);
        }
        catch (Exception ex)
        {
            if (configuration.Value.ApplicationType == SidekickApplicationType.DataBuilder ||
                configuration.Value.ApplicationType == SidekickApplicationType.Test)
                throw;
            logger.LogError(ex, $"Failed to download ninja data for {language.Code}.");
        }
    }

    private async Task DownloadExchange(GameType game, string league)
    {
        var pages = game == GameType.PathOfExile1 ? Poe1Pages : Poe2Pages;

        using var http = CreateHttpClient();
        await using var db = new NinjaDbContext(dbContextOptions);

        foreach (var page in pages)
        {
            if (!page.SupportsExchange) continue;

            var url = $"https://poe.ninja/{game.GetValueAttribute()}/api/economy/exchange/current/overview?league={league.Replace(" ", "+")}&type={page.Type}";
            var items = await DownloadExchangePage(http, url);
            if (items == null) continue;

            foreach (var item in items)
            {
                db.ExchangeItems.Add(new NinjaExchangeItem
                {
                    UniqueId = Guid.NewGuid(),
                    Game = game,
                    Type = page.Type,
                    Id = item.Id,
                    DetailsId = item.DetailsId,
                });
            }

            await db.SaveChangesAsync();
            logger.LogInformation($"Downloaded {items.Count} exchange items for {page.Type}/{game}");
        }
    }

    private async Task DownloadStash(GameType game, string league)
    {
        var pages = game == GameType.PathOfExile1 ? Poe1Pages : Poe2Pages;

        using var http = CreateHttpClient();
        await using var db = new NinjaDbContext(dbContextOptions);

        foreach (var page in pages)
        {
            if (!page.SupportsStash || page.SupportsExchange) continue;

            var url = $"https://poe.ninja/{game.GetValueAttribute()}/api/economy/stash/current/item/overview?league={league.Replace(" ", "+")}&type={page.Type}";
            var items = await DownloadStashPage(http, url);
            if (items == null) continue;

            foreach (var item in items)
            {
                db.StashItems.Add(new NinjaStashItem
                {
                    UniqueId = Guid.NewGuid(),
                    Game = game,
                    Type = page.Type,
                    DetailsId = item.DetailsId,
                    Name = item.Name,
                    BaseType = item.BaseType,
                    Corrupted = item.Corrupted,
                    GemLevel = item.GemLevel,
                    GemQuality = item.GemQuality,
                    Links = item.Links,
                    LevelRequired = item.LevelRequired,
                    Variant = item.Variant,
                });
            }

            await db.SaveChangesAsync();
            logger.LogInformation($"Downloaded {items.Count} stash items for {page.Type}/{game}");
        }
    }

    private async Task<List<NinjaExchangeItemDto>?> DownloadExchangePage(HttpClient http, string url)
    {
        try
        {
            logger.LogDebug($"GET {url}");
            var json = await http.GetStringAsync(url);
            var result = System.Text.Json.JsonSerializer.Deserialize<NinjaExchangeOverview>(json, JsonOptions);
            return result?.Items;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed for {url}: {ex.Message}");
            return null;
        }
    }

    private async Task<List<NinjaStashLineDto>?> DownloadStashPage(HttpClient http, string url)
    {
        try
        {
            logger.LogDebug($"GET {url}");
            var json = await http.GetStringAsync(url);
            var result = System.Text.Json.JsonSerializer.Deserialize<NinjaStashOverview>(json, JsonOptions);
            return result?.Lines;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed for {url}: {ex.Message}");
            return null;
        }
    }

    private static HttpClient CreateHttpClient()
    {
        var http = new HttpClient();
        http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Sidekick.Data", "1.0"));
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
        return http;
    }

    private static readonly System.Text.Json.JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

}
