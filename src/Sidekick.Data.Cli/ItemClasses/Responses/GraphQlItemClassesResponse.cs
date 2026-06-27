using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.ItemClasses.Responses;

public class GraphQlItemClassesResponse
{
    [JsonPropertyName("poe1_itemClasses")] public List<GraphQlItemClass>? Poe1ItemClasses { get; set; }
    [JsonPropertyName("poe2_itemClasses")] public List<GraphQlItemClass>? Poe2ItemClasses { get; set; }
}