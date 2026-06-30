using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.Uniques.Responses;

public class GraphQlUniqueItemsResponse
{
    [JsonPropertyName("poe1_uniqueStashLayout")]
    public List<GraphQlUniqueItem>? Poe1 { get; set; }

    [JsonPropertyName("poe2_uniqueStashLayout")]
    public List<GraphQlUniqueItem>? Poe2 { get; set; }
}