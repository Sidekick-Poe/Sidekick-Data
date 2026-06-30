using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Common.Enums;
using Sidekick.Data.Cli.Ninja.Dtos;
using Sidekick.Data.Ninja;

namespace Sidekick.Data.Cli.Ninja;

public class NinjaDownloader(
    ILogger<NinjaDownloader> logger,
    DbContextOptions<DataDbContext> dbContextOptions)
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

    public async Task Download(GameType game)
    {
        await using var db = new DataDbContext(dbContextOptions);
        var league = await db.TradeLeagues.FirstAsync(x => x.Game == game && x.Language == "en");

        await DownloadExchange(game, league.Id);
        await DownloadStash(game, league.Id);
    }

    private async Task DownloadExchange(GameType game, string league)
    {
        var pages = game == GameType.PathOfExile1 ? Poe1Pages : Poe2Pages;

        using var http = CreateHttpClient();
        await using var db = new DataDbContext(dbContextOptions);
        db.NinjaExchangeItems.RemoveRange(db.NinjaExchangeItems.Where(x => x.Game == game));
        await db.SaveChangesAsync();

        foreach (var page in pages)
        {
            if (!page.SupportsExchange) continue;

            var url =
                $"https://poe.ninja/{game.GetValueAttribute()}/api/economy/exchange/current/overview?league={league.Replace(" ", "+")}&type={page.Type}";
            var items = await DownloadExchangePage(http, url);
            if (items == null) continue;

            foreach (var item in items)
            {
                db.NinjaExchangeItems.Add(new NinjaExchangeItem
                {
                    SidekickId = Guid.NewGuid(),
                    Game = game,
                    Type = page.Type,
                    Url = page.Url,
                    Id = item.Id,
                    DetailsId = item.DetailsId,
                });
            }

            await db.SaveChangesAsync();
            logger.LogInformation($"[NinjaDownloader] Downloaded {items.Count} exchange items for {page.Type}/{game}");
        }
    }

    private async Task DownloadStash(GameType game, string league)
    {
        var pages = game == GameType.PathOfExile1 ? Poe1Pages : Poe2Pages;

        using var http = CreateHttpClient();
        await using var db = new DataDbContext(dbContextOptions);
        db.NinjaStashItems.RemoveRange(db.NinjaStashItems.Where(x => x.Game == game));
        await db.SaveChangesAsync();

        foreach (var page in pages)
        {
            if (!page.SupportsStash || page.SupportsExchange) continue;

            var url =
                $"https://poe.ninja/{game.GetValueAttribute()}/api/economy/stash/current/item/overview?league={league.Replace(" ", "+")}&type={page.Type}";
            var items = await DownloadStashPage(http, url);
            if (items == null) continue;

            foreach (var item in items)
            {
                var itemId = Guid.NewGuid();
                db.NinjaStashItems.Add(new NinjaStashItem
                {
                    SidekickId = itemId,
                    Game = game,
                    Type = page.Type,
                    Url = page.Url,
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

                if (item.TradeInfo != null)
                {
                    foreach (var stat in item.TradeInfo)
                    {
                        if (stat.Mod == null)
                        {
                            Debugger.Break();
                        }

                        db.NinjaStashTradeStats.Add(new NinjaStashTradeStat
                        {
                            SidekickId = Guid.NewGuid(),
                            Mod = stat.Mod,
                            Min = stat.Min,
                            Max = stat.Max,
                            Option = stat.Option,
                            StashItemId = itemId,
                        });
                    }
                }

                if (item.MutatedModifiers != null)
                {
                    foreach (var stat in item.MutatedModifiers)
                    {
                        db.NinjaStashMutatedStats.Add(new NinjaStashMutatedStat
                        {
                            SidekickId = Guid.NewGuid(),
                            Text = stat.Text,
                            Optional = stat.Optional,
                            StashItemId = itemId,
                        });
                    }
                }
            }

            await db.SaveChangesAsync();
            logger.LogInformation($"[NinjaDownloader] Downloaded {items.Count} stash items for {page.Type}/{game}");
        }
    }

    private async Task<List<NinjaExchangeItemDto>?> DownloadExchangePage(HttpClient http, string url)
    {
        logger.LogDebug($"GET {url}");
        var json = await http.GetStringAsync(url);
        var result = System.Text.Json.JsonSerializer.Deserialize<NinjaExchangeOverview>(json, JsonOptions);
        return result?.Items;
    }

    private async Task<List<NinjaStashLineDto>?> DownloadStashPage(HttpClient http, string url)
    {
        logger.LogDebug($"GET {url}");
        var json = await http.GetStringAsync(url);
        var result = System.Text.Json.JsonSerializer.Deserialize<NinjaStashOverview>(json, JsonOptions);
        return result?.Lines;
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