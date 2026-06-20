using Sidekick.Data.Leagues;

namespace Sidekick.Data.Cli.Trade.Models;

public class RawTradeLeague
{
    public string? Id { get; set; }

    public string? Text { get; set; }

    public LeagueRealm Realm { get; set; }
}
