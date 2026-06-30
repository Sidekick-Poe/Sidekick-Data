using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlAttributeRequirementPoe1 : IGraphQlAttributeRequirement
{
    [JsonPropertyName("BaseItemTypesKey")]
    public required string BaseItemId { get; set; }

    [JsonPropertyName("ReqDex")]
    public int RequiresDexterity { get; set; }

    [JsonPropertyName("ReqInt")]
    public int RequiresIntelligence { get; set; }

    [JsonPropertyName("ReqStr")]
    public int RequiresStrength { get; set; }
}