using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Data.Files;
using Sidekick.Data.Options;

namespace Sidekick.Data.Trade;

internal class TradeDownloader(
    ILogger<TradeDownloader> logger,
    DataFileWriter dataFileWriter,
    IOptions<DataOptions> options)
{
    private static readonly List<LanguageInfo> Languages = new()
    {
        new("en",
            Poe1Api: "https://www.pathofexile.com/api/trade/",
            Poe2Api: "https://www.pathofexile.com/api/trade2/"),
        new("de",
            Poe1Api: "https://de.pathofexile.com/api/trade/",
            Poe2Api: "https://de.pathofexile.com/api/trade2/"),
        new("es",
            Poe1Api: "https://es.pathofexile.com/api/trade/",
            Poe2Api: "https://es.pathofexile.com/api/trade2/"),
        new("fr",
            Poe1Api: "https://fr.pathofexile.com/api/trade/",
            Poe2Api: "https://fr.pathofexile.com/api/trade2/"),
        new("ja",
            Poe1Api: "https://jp.pathofexile.com/api/trade/",
            Poe2Api: "https://jp.pathofexile.com/api/trade2/"),
        new("ko",
            Poe1Api: "https://poe.game.daum.net/api/trade/",
            Poe2Api: "https://poe.game.daum.net/api/trade2/"),
        new("pt",
            Poe1Api: "https://br.pathofexile.com/api/trade/",
            Poe2Api: "https://br.pathofexile.com/api/trade2/"),
        new("ru",
            Poe1Api: "https://ru.pathofexile.com/api/trade/",
            Poe2Api: "https://ru.pathofexile.com/api/trade2/"),
        new("th",
            Poe1Api: "https://th.pathofexile.com/api/trade/",
            Poe2Api: "https://th.pathofexile.com/api/trade2/"),
        new("zh",
            Poe1Api: "http://www.pathofexile.tw/api/trade/",
            Poe2Api: "http://www.pathofexile.tw/api/trade2/"),
    };

    private static string GetFileName(string game, string langCode, string path)
        => $"{game}.{langCode}.{path}.json";

    private static string GetApiBase(string langCode, string game)
    {
        var lang = Languages.First(l => l.Code == langCode);
        return game == "poe2" ? lang.Poe2Api : lang.Poe1Api;
    }

    public async Task DownloadAll()
    {
        using var http = new HttpClient();
        http.Timeout = TimeSpan.FromSeconds(options.Value.TimeoutSeconds);
        http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Sidekick.Data", "1.0"));
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");

        foreach (var game in new[] { "poe1", "poe2" })
        {
            // Download leagues once (English, invariant)
            var leaguesUrl = GetApiBase("en", game) + "data/leagues";
            await DownloadToFile(http, leaguesUrl, GetFileName(game, "en", "leagues"));

            foreach (var code in options.Value.LanguageCodes)
            {
                var lang = Languages.FirstOrDefault(x => x.Code == code);
                if (lang == null)
                {
                    logger.LogWarning($"[Trade] Skipping unsupported language code '{code}'.");
                    continue;
                }

                foreach (var path in options.Value.TradePaths)
                {
                    var url = GetApiBase(lang.Code, game) + "data/" + path;
                    await DownloadToFile(http, url, GetFileName(game, lang.Code, path));
                }
            }
        }
    }

    private async Task DownloadToFile(HttpClient http, string url, string fileName)
    {
        try
        {
            logger.LogInformation($"[Trade] GET {url}");
            using var response = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            await dataFileWriter.Write(fileName, await response.Content.ReadAsStreamAsync());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"[Trade] Failed for {url}: {ex.Message}");
        }
    }

    private sealed record LanguageInfo(string Code, string Poe1Api, string Poe2Api);

}