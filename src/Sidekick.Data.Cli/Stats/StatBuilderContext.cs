using Sidekick.Data.Cli.GraphQl.Models;
using Sidekick.Data.Languages;
using Sidekick.Data.StatsInvariant;
using Sidekick.Data.Trade;

namespace Sidekick.Data.Cli.Stats;

public record StatBuilderContext(
    GameType Game,
    IGameLanguage Language,
    StatPatternBuilder PatternBuilder,
    List<GraphQlStatDescription> GraphQlStatDescriptions,
    List<TradeStatDefinition> TradeDefinitions,
    StatsInvariantDetails InvariantDetails);
