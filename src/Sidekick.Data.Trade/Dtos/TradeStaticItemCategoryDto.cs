namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeStaticItemCategoryDto
{
    public string? Id { get; init; }
    public string? Label { get; init; }
    public List<TradeStaticItemDto> Entries { get; init; } = new();
}
