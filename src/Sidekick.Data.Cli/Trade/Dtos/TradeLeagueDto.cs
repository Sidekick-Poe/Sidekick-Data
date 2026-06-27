using Sidekick.Data.Trade;

namespace Sidekick.Data.Cli.Trade.Dtos;

internal sealed record TradeLeagueDto
{
    public string? Id { get; init; }
    public string? Text { get; init; }
    public TradeLeagueRealm Realm { get; init; }
}
