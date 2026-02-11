using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sidekick.Data;

internal static class TradeDownloader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

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

    public static async Task DownloadAll(CommandOptions options)
    {
        Directory.CreateDirectory(options.DataFolder);

        using var http = CreateHttpClient(options.TimeoutSeconds);

        foreach (var game in new[] { "poe1", "poe2" })
        {
            // Download leagues once (English, invariant)
            var leaguesUrl = GetApiBase("en", game) + "data/leagues";
            var leaguesFile = Path.Combine(options.DataFolder, GetFileName(game, "en", "leagues"));
            await DownloadToFile(http, leaguesUrl, leaguesFile);

            foreach (var code in options.LanguageCodes)
            {
                var lang = Languages.FirstOrDefault(x => x.Code == code);
                if (lang == null)
                {
                    Console.WriteLine($"[Trade] Skipping unsupported language code '{code}'.");
                    continue;
                }

                foreach (var path in options.TradePaths)
                {
                    var url = GetApiBase(lang.Code, game) + "data/" + path;
                    var file = Path.Combine(options.DataFolder, GetFileName(game, lang.Code, path));
                    await DownloadToFile(http, url, file);
                }
            }
        }
    }

    private static HttpClient CreateHttpClient(int timeoutSeconds)
    {
        var http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(timeoutSeconds)
        };
        http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Sidekick.Data", "1.0"));
        http.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
        return http;
    }

    private static async Task DownloadToFile(HttpClient http, string url, string filePath)
    {
        try
        {
            Console.WriteLine($"[Trade] GET {url}");
            using var response = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var output = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(output);
            Console.WriteLine($"[Trade] Saved {Path.GetFileName(filePath)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Trade] Failed for {url}: {ex.Message}");
        }
    }

    private static string GetApiBase(string langCode, string game)
    {
        var lang = Languages.First(l => l.Code == langCode);
        return game == "poe2" ? lang.Poe2Api : lang.Poe1Api;
    }

    private static string GetFileName(string game, string langCode, string path)
        => $"{game}.{langCode}.{path}.json";

    private sealed record LanguageInfo(string Code, string Poe1Api, string Poe2Api);

}