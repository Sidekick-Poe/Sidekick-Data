using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.GraphQl.Models;

public class GraphQlItemClass
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}

public class ItemClassesResponse
{
    [JsonPropertyName("poe1_itemClasses")] public List<GraphQlItemClass>? Poe1ItemClasses { get; set; }
    [JsonPropertyName("poe2_itemClasses")] public List<GraphQlItemClass>? Poe2ItemClasses { get; set; }
}
