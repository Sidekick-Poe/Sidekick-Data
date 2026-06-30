using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlAttributeRequirementPoe2 : IGraphQlAttributeRequirement
{
    [JsonPropertyName("BaseItemType")]
    public required GraphQlBaseItem BaseItem { get; set; }

    public string BaseItemId => BaseItem.Id;

    [JsonPropertyName("ReqDex")]
    public int RequiresDexterity { get; set; }

    [JsonPropertyName("ReqInt")]
    public int RequiresIntelligence { get; set; }

    [JsonPropertyName("ReqStr")]
    public int RequiresStrength { get; set; }
}