using Microsoft.Extensions.Logging;
using Sidekick.Data.Cli.ItemClasses;
using Sidekick.Data.Cli.ItemDefinitions;
using Sidekick.Data.Cli.Leagues;
using Sidekick.Data.Cli.Ninja;
using Sidekick.Data.Cli.Pseudo;
using Sidekick.Data.Cli.Stats;
using Sidekick.Data.Cli.StatsInvariant;
using Sidekick.Data.Cli.Trade;
using Sidekick.Data.Languages;

namespace Sidekick.Data.Cli;

public class DataBuilder(
    ILogger<DataBuilder> logger,
    LeagueBuilder leagueBuilder,
    NinjaDownloader ninjaDownloader,
    StatBuilder statBuilder,
    PseudoBuilder pseudoBuilder,
    TradeDownloader tradeDownloader,
    ItemDefinitionBuilder itemDefinitionBuilder,
    ItemClassBuilder itemClassBuilder,
    StatsInvariantBuilder statsInvariantBuilder,
    TradeFilterBuilder tradeFilterBuilder,
    TradeStatBuilder tradeStatBuilder,
    IGameLanguageProvider gameLanguageProvider)
{
    public async Task DownloadRawFiles(
        IGameLanguage language,
        bool trade = true,
        bool ninja = true)
    {
        logger.LogInformation($"Downloading {language.Code} data files.");

        if (trade)
        {
            logger.LogInformation($"Downloading {language.Code} trade data.");
            await tradeDownloader.Download(language);
            logger.LogInformation($"Downloaded {language.Code} trade data.");
        }

        // RePoE download step removed — game data is now fetched from the local GraphQL API at build time.

        if (ninja)
        {
            if (language.Code != gameLanguageProvider.InvariantLanguage.Code) return;
            logger.LogInformation("Downloading ninja data.");
            await ninjaDownloader.Download();
            logger.LogInformation($"Downloaded {language.Code} ninja data.");
        }

        logger.LogInformation($"Downloaded {language.Code} data files.");
    }

    public async Task BuildDataFiles(
        IGameLanguage language,
        bool items = true,
        bool stats = true,
        bool trade = true,
        bool pseudo = true)
    {
        logger.LogInformation($"Building {language.Code} data files.");

        if (trade)
        {
            logger.LogInformation($"Building {language.Code} trade data.");
            await leagueBuilder.Build(language);
            await tradeFilterBuilder.Build(language);
            await tradeStatBuilder.Build(language);
            await statsInvariantBuilder.Build(language);
            logger.LogInformation($"Built {language.Code} trade data.");
        }

        if (items)
        {
            logger.LogInformation($"Building {language.Code} items data.");
            await itemClassBuilder.Build(language);
            await itemDefinitionBuilder.Build(language);
            logger.LogInformation($"Built {language.Code} items data.");
        }

        if (pseudo)
        {
            logger.LogInformation($"Building {language.Code} pseudo data.");
            await pseudoBuilder.Build(language);
            logger.LogInformation($"Built {language.Code} pseudo data.");
        }

        if (stats)
        {
            logger.LogInformation($"Building {language.Code} stats data.");
            await statBuilder.Build(language);
            logger.LogInformation($"Built {language.Code} stats data.");
        }

        logger.LogInformation($"Built {language.Code} data files.");
    }
}
