namespace Sidekick.Data.Cli.Trade.Dtos;

internal sealed record TradeStaticItemDto
{
    public required string Id { get; init; }
    public string? Text { get; init; }
    public string? Image { get; init; }
}
