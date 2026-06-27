using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Sidekick.Common;
using Sidekick.Data;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.ItemClasses;
using Sidekick.Data.Cli.Ninja;
using Sidekick.Data.Cli.StatsInvariant;
using Sidekick.Data.Cli.Trade;
using Sidekick.Data.Languages;

#region Services

var services = new ServiceCollection();
services.AddLogging(o =>
{
    o.SetMinimumLevel(LogLevel.Trace);
    o.AddFilter("Microsoft", LogLevel.Warning);
    o.AddFilter("System", LogLevel.Warning);
    o.AddConsole();
});

services.AddSidekickCommon(SidekickApplicationType.DataBuilder);
services.AddSidekickData();

services.TryAddSingleton<GraphQlClient>();
services.TryAddSingleton<NinjaDownloader>();
services.TryAddSingleton<StatsInvariantBuilder>();
services.TryAddSingleton<TradeApiDownloader>();
services.TryAddSingleton<ItemClassBuilder>();

var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var ninjaDownloader = serviceProvider.GetRequiredService<NinjaDownloader>();
var gameLanguageProvider = serviceProvider.GetRequiredService<IGameLanguageProvider>();
var statsInvariantBuilder = serviceProvider.GetRequiredService<StatsInvariantBuilder>();
var tradeApiDownloader = serviceProvider.GetRequiredService<TradeApiDownloader>();
var itemClassBuilder = serviceProvider.GetRequiredService<ItemClassBuilder>();

#endregion

#region Configuration

var download = false;
var build = false;

var runPoe1 = false;
var runPoe2 = false;

var runLanguage = string.Empty;
var runItems = false;
var runStats = false;
var runTrade = false;
var runPseudo = false;
var runNinja = false;

for (var i = 0; i < args.Length; i++)
{
    var a = args[i];
    switch (a)
    {
        case "--language" when i + 1 < args.Length:
            runLanguage = args[++i];
            break;
        case "--items":
            runItems = true;
            break;
        case "--stats":
            runStats = true;
            break;
        case "--trade":
            runTrade = true;
            break;
        case "--pseudo":
            runPseudo = true;
            break;
        case "--ninja":
            runNinja = true;
            break;
        case "--download":
            download = true;
            break;
        case "--build":
            build = true;
            break;
        case "--poe1":
            runPoe1 = true;
            break;
        case "--poe2":
            runPoe2 = true;
            break;
    }
}

if (!runItems && !runStats && !runTrade && !runPseudo && !runNinja)
{
    runItems = true;
    runStats = true;
    runTrade = true;
    runPseudo = true;
    runNinja = true;
}

if (!runPoe1 && !runPoe2)
{
    runPoe1 = true;
    runPoe2 = true;
}

if (!build && !download)
{
    logger.LogCritical("Specify either --download or --build");
}

#endregion

foreach (var game in new[] { GameType.PathOfExile1, GameType.PathOfExile2 })
{
    switch (game)
    {
        case GameType.PathOfExile1:
            if (!runPoe1) continue;
            break;
        case GameType.PathOfExile2:
            if (!runPoe2) continue;
            break;
    }

    foreach (var language in gameLanguageProvider.GetList())
    {
        if (!string.IsNullOrEmpty(runLanguage) && language.Code != runLanguage) continue;

        if (download && runTrade)
        {
            logger.LogInformation($"Downloading {game}/{language.Code} trade data.");
            await tradeApiDownloader.Download(game, language);
            logger.LogInformation($"Downloaded {game}/{language.Code} trade data.");
        }

        if (download && runNinja && language.Code == gameLanguageProvider.InvariantLanguage.Code)
        {
            logger.LogInformation($"Downloading {game} ninja data.");
            await ninjaDownloader.Download(game);
            logger.LogInformation($"Downloaded {game} ninja data.");
        }

        if (build && runTrade && language.Code == gameLanguageProvider.InvariantLanguage.Code)
        {
            logger.LogInformation($"Building {game} invariant stats.");
            await statsInvariantBuilder.Build(game);
            logger.LogInformation($"Built {game} invariant stats.");
        }

        if (build && runItems)
        {
            logger.LogInformation($"Building {language.Code} item classes.");
            await itemClassBuilder.Build(game, language);
            logger.LogInformation($"Built {language.Code} item classes.");
        }
    }
}
