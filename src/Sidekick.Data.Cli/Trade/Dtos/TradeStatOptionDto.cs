namespace Sidekick.Data.Cli.Trade.Dtos;

internal sealed record TradeStatOptionDto
{
    public int Id { get; init; }
    public required string Text { get; init; }
}