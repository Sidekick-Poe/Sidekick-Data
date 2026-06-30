using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlAttributeRequirementsResponse
{
    [JsonPropertyName("poe1_componentAttributeRequirements")]
    public List<GraphQlAttributeRequirementPoe1>? Poe1 { get; set; }

    [JsonPropertyName("poe2_attributeRequirements")]
    public List<GraphQlAttributeRequirementPoe2>? Poe2 { get; set; }
}