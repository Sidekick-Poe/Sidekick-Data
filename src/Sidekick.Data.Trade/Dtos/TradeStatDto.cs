using System.Text.Json.Serialization;

namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeStatDto
{
    public required string Id { get; init; }
    public required string Text { get; init; }
    public required string Type { get; init; }

    [JsonPropertyName("option")]
    public TradeStatOptionGroupDto? Option { get; init; }
}

internal sealed record TradeStatOptionGroupDto
{
    public List<TradeStatOptionDto> Options { get; init; } = new();
}

internal sealed record TradeStatOptionDto
{
    public int Id { get; init; }
    public required string Text { get; init; }
}
