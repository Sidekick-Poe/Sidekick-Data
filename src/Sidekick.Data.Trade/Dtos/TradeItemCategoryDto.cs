namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeItemCategoryDto
{
    public string? Id { get; init; }
    public string? Label { get; init; }
    public List<TradeItemDto> Entries { get; init; } = new();
}
