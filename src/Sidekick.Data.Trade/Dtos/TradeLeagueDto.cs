using Sidekick.Data.Leagues;

namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeLeagueDto
{
    public string? Id { get; init; }
    public string? Text { get; init; }
    public LeagueRealm Realm { get; init; }
}
