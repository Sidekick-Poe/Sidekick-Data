using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Sidekick.Common;
using Sidekick.Data;
using Sidekick.Data.Cli.BaseItems;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.ItemClasses;
using Sidekick.Data.Cli.Ninja;
using Sidekick.Data.Cli.StatsInvariant;
using Sidekick.Data.Cli.Trade;
using Sidekick.Data.Cli.Uniques;
using Sidekick.Data.Languages;
using Sidekick.Game;

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
services.AddSidekickGame();
services.AddSidekickData();

services.TryAddSingleton<GraphQlClient>();
services.TryAddSingleton<NinjaDownloader>();
services.TryAddSingleton<StatsInvariantBuilder>();
services.TryAddSingleton<TradeApiDownloader>();
services.TryAddSingleton<ItemClassBuilder>();
services.TryAddSingleton<BaseItemBuilder>();
services.TryAddSingleton<UniqueItemBuilder>();

var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var ninjaDownloader = serviceProvider.GetRequiredService<NinjaDownloader>();
var gameLanguageProvider = serviceProvider.GetRequiredService<IGameLanguageProvider>();
var statsInvariantBuilder = serviceProvider.GetRequiredService<StatsInvariantBuilder>();
var tradeApiDownloader = serviceProvider.GetRequiredService<TradeApiDownloader>();
var itemClassBuilder = serviceProvider.GetRequiredService<ItemClassBuilder>();
var baseItemBuilder = serviceProvider.GetRequiredService<BaseItemBuilder>();
var uniqueItemBuilder = serviceProvider.GetRequiredService<UniqueItemBuilder>();

#endregion

#region Configuration

var download = false;
var build = false;

var runPoe1 = false;
var runPoe2 = false;

var runLanguage = string.Empty;
var runItemClasses = false;
var runBaseItems = false;
var runUniqueItems = false;
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
        case "--item-classes":
            runItemClasses = true;
            break;
        case "--base-items":
            runBaseItems = true;
            break;
        case "--unique-items":
            runUniqueItems = true;
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

if (!runItemClasses && !runBaseItems && !runStats && !runTrade && !runPseudo && !runNinja)
{
    runItemClasses = true;
    runBaseItems = true;
    runUniqueItems = true;
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

    foreach (var language in gameLanguageProvider.GetList()
                 .OrderBy(x => x.Code == "en" ? 0 : 1)
                 .ThenBy(x => x.Code))
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

        if (build && runItemClasses)
        {
            logger.LogInformation($"Building {game}/{language.Code} item classes.");
            await itemClassBuilder.Build(game, language);
            logger.LogInformation($"Built {game}/{language.Code} item classes.");
        }

        if (build && runBaseItems)
        {
            logger.LogInformation($"Building {game}/{language.Code} base item types.");
            await baseItemBuilder.Build(game, language);
            logger.LogInformation($"Built {game}/{language.Code} base item types.");
        }

        if (build && runUniqueItems)
        {
            logger.LogInformation($"Building {game}/{language.Code} unique items.");
            await uniqueItemBuilder.Build(game, language);
            logger.LogInformation($"Built {game}/{language.Code} unique items.");
        }
    }
}