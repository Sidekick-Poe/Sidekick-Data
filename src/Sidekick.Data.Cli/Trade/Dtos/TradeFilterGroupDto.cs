namespace Sidekick.Data.Cli.Trade.Dtos;

internal sealed record TradeFilterGroupDto
{
    public string? Id { get; init; }
    public string? Text { get; init; }
    public string? Type { get; init; }
    public List<TradeFilterDto> Filters { get; init; } = new();
}
