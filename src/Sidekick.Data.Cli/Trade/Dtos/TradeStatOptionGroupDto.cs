namespace Sidekick.Data.Cli.Trade.Dtos;

internal sealed record TradeStatOptionGroupDto
{
    public List<TradeStatOptionDto> Options { get; init; } = new();
}