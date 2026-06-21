using System.Text.Json.Serialization;

namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeItemDto
{
    public string? Name { get; init; }
    public string? Type { get; init; }
    public string? Text { get; init; }

    [JsonPropertyName("disc")]
    public string? Discriminator { get; init; }

    [JsonPropertyName("flags")]
    public TradeItemFlagsDto? Flags { get; init; }
}

internal sealed record TradeItemFlagsDto
{
    public bool Unique { get; init; }
}
