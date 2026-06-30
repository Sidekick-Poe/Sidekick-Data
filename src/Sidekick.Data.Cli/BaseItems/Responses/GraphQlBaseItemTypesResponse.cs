using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlBaseItemTypesResponse
{
    [JsonPropertyName("poe1_baseItemTypes")]
    public List<GraphQlBaseItem>? Poe1 { get; set; }

    [JsonPropertyName("poe2_baseItemTypes")]
    public List<GraphQlBaseItem>? Poe2 { get; set; }
}