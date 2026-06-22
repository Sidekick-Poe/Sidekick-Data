using System.Text.Json.Serialization;

namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeFilterDto
{
    public required string Id { get; init; }
    public string? Text { get; init; }
    public string? Type { get; init; }
    public bool? Hidden { get; init; }
    public bool? FullSpan { get; init; }
    public bool? HalfSpan { get; init; }
    public bool? MinMax { get; init; }
    public bool? Sockets { get; init; }
    public string? Tip { get; init; }

    [JsonPropertyName("option")]
    public TradeFilterOptionGroupDto? Option { get; init; }
}