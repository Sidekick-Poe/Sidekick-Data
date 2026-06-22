namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeFilterOptionGroupDto
{
    public List<TradeFilterOptionDto> Options { get; init; } = new();
}