namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeStatCategoryDto
{
    public string? Id { get; init; }
    public string? Label { get; init; }
    public List<TradeStatDto> Entries { get; init; } = new();
}
