namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeStatOptionGroupDto
{
    public List<TradeStatOptionDto> Options { get; init; } = new();
}