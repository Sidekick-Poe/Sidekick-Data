using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlArmourTypesResponse
{
    [JsonPropertyName("poe1_armourTypes")]
    public List<GraphQlArmourTypePoe1>? Poe1 { get; set; }

    [JsonPropertyName("poe2_armourTypes")]
    public List<GraphQlArmourTypePoe2>? Poe2 { get; set; }
}