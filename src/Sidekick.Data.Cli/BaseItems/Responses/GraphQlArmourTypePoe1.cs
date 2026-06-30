using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlArmourTypePoe1 : IGraphQlArmourType
{
    [JsonPropertyName("BaseItemTypesKey")]
    public required GraphQlBaseItem BaseItem { get; set; }

    [JsonPropertyName("ArmourMin")]
    public int? ArmourMin { get; set; }

    [JsonPropertyName("ArmourMax")]
    public int? ArmourMax { get; set; }

    [JsonPropertyName("EvasionMin")]
    public int? EvasionMin { get; set; }

    [JsonPropertyName("EvasionMax")]
    public int? EvasionMax { get; set; }

    [JsonPropertyName("EnergyShieldMin")]
    public int? EnergyShieldMin { get; set; }

    [JsonPropertyName("EnergyShieldMax")]
    public int? EnergyShieldMax { get; set; }

    [JsonPropertyName("WardMin")]
    public int? WardMin { get; set; }

    [JsonPropertyName("WardMax")]
    public int? WardMax { get; set; }
}