using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Data.Cli.StatsInvariant;
using Sidekick.Data.Cli.Trade.Models;
using Sidekick.Data.Extensions;
using Sidekick.Data.Languages;
using Sidekick.Data.Trade;

namespace Sidekick.Data.Cli.Trade;

public class TradeStatBuilder(
    ILogger<StatsInvariantBuilder> logger,
    IOptions<SidekickConfiguration> configuration,
    DataProvider dataProvider)
{
    public async Task Build(IGameLanguage language)
    {
        try
        {
            await BuildForGame(GameType.PathOfExile1, language);
            await BuildForGame(GameType.PathOfExile2, language);
        }
        catch (Exception ex)
        {
            if (configuration.Value.ApplicationType == SidekickApplicationType.DataBuilder || configuration.Value.ApplicationType == SidekickApplicationType.Test)
            {
                throw;
            }

            logger.LogError(ex, "Failed to build trade filters.");
        }
    }

    private async Task BuildForGame(GameType game, IGameLanguage language)
    {
        var apiCategories = await dataProvider.Read<RawTradeResult<List<RawTradeStatCategory>>>(game, DataType.RawTradeStats, language);

        var stats = new List<TradeStatDefinition>();

        foreach (var apiCategory in apiCategories.Result)
        {
            if (apiCategory.Entries.Count == 0) continue;

            foreach (var entry in apiCategory.Entries)
            {
                if (string.IsNullOrEmpty(entry.Id)) continue;

                stats.AddRange(BuildStat(entry));
            }
        }

        await dataProvider.Write(game, DataType.TradeStats, language, stats);
    }

    private IEnumerable<TradeStatDefinition> BuildStat(RawTradeStat entry)
    {
        if (entry.Options?.Options.Count > 0)
        {
            foreach (var option in entry.Options.Options)
            {
                yield return new TradeStatDefinition
                {
                    Id = $"{entry.Id}#{option.Id}",
                    Text = entry.Text.RemoveSquareBrackets(),
                    OptionText = option.Text.RemoveSquareBrackets(),
                };
            }
            yield break;
        }

        yield return new TradeStatDefinition()
        {
            Id = entry.Id,
            Text = entry.Text.RemoveSquareBrackets(),
        };
    }
}
