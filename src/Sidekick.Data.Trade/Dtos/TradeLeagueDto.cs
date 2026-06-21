namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeLeagueDto
{
    public string Id { get; init; }
    public string? Text { get; init; }
    public string? Realm { get; init; }
}
