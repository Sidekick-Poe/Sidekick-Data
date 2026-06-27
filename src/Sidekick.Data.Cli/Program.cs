using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Sidekick.Common;
using Sidekick.Data;
using Sidekick.Data.Cli;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.ItemClasses;
using Sidekick.Data.Cli.ItemDefinitions;
using Sidekick.Data.Cli.Ninja;
using Sidekick.Data.Cli.Pseudo;
using Sidekick.Data.Cli.Stats;
using Sidekick.Data.Cli.StatsInvariant;
using Sidekick.Data.Cli.Trade;
using Sidekick.Data.Languages;
using Sidekick.Data.Ninja;
using Sidekick.Data.Trade;

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
services.AddSidekickDataNinja();
services.AddSidekickDataTrade();

services.TryAddSingleton<GraphQlClient>();
services.TryAddSingleton<NinjaDownloader>();
services.TryAddSingleton<PseudoBuilder>();
services.TryAddSingleton<StatBuilder>();
services.TryAddSingleton<ItemClassBuilder>();
services.TryAddSingleton<ItemDefinitionBuilder>();
services.TryAddSingleton<StatsInvariantBuilder>();
services.TryAddSingleton<RawDataProvider>();
services.TryAddSingleton<TradeApiDownloader>();

var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var ninjaDownloader = serviceProvider.GetRequiredService<NinjaDownloader>();
var statBuilder = serviceProvider.GetRequiredService<StatBuilder>();
var pseudoBuilder = serviceProvider.GetRequiredService<PseudoBuilder>();
var itemDefinitionBuilder = serviceProvider.GetRequiredService<ItemDefinitionBuilder>();
var itemClassBuilder = serviceProvider.GetRequiredService<ItemClassBuilder>();
var statsInvariantBuilder = serviceProvider.GetRequiredService<StatsInvariantBuilder>();
var gameLanguageProvider = serviceProvider.GetRequiredService<IGameLanguageProvider>();
var tradeApiDownloader = serviceProvider.GetRequiredService<TradeApiDownloader>();

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

#endregion

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
            // await leagueBuilder.Build(language);
            // await tradeDownloader.Download(language);
            logger.LogInformation($"Downloaded {game}/{language.Code} trade data.");
        }

        if (download && runNinja && language.Code == gameLanguageProvider.InvariantLanguage.Code)
        {
            logger.LogInformation($"Downloading {game} ninja data.");
            await ninjaDownloader.Download(game);
            logger.LogInformation($"Downloaded {game} ninja data.");
        }

        // if (build && runTrade)
        // {
        //     logger.LogInformation($"Building {language.Code} trade data.");
        //     await tradeFilterBuilder.Build(language);
        //     await tradeStatBuilder.Build(language);
        //     await statsInvariantBuilder.Build(language);
        //     logger.LogInformation($"Built {language.Code} trade data.");
        // }
//
        // if (build && runItems)
        // {
        //     logger.LogInformation($"Building {language.Code} items data.");
        //     await itemClassBuilder.Build(language);
        //     await itemDefinitionBuilder.Build(language);
        //     logger.LogInformation($"Built {language.Code} items data.");
        // }
//
        // if (build && runPseudo)
        // {
        //     logger.LogInformation($"Building {language.Code} pseudo data.");
        //     await pseudoBuilder.Build(language);
        //     logger.LogInformation($"Built {language.Code} pseudo data.");
        // }
//
        // if (build && runStats)
        // {
        //     logger.LogInformation($"Building {language.Code} stats data.");
        //     await statBuilder.Build(language);
        //     logger.LogInformation($"Built {language.Code} stats data.");
        // }
//
        // if (download && runNinja)
        // {
        //     if (language.Code != gameLanguageProvider.InvariantLanguage.Code) continue;
        //     logger.LogInformation("Downloading ninja data.");
        //     await ninjaDownloader.Download();
        //     logger.LogInformation($"Downloaded {language.Code} ninja data.");
        // }
    }
}