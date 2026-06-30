using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlArmourTypePoe2 : IGraphQlArmourType
{
    [JsonPropertyName("BaseItemType")]
    public required GraphQlBaseItem BaseItem { get; set; }

    [JsonPropertyName("Armour")]
    public int? Armour { get; set; }

    public int? ArmourMin => Armour;
    public int? ArmourMax => Armour;

    [JsonPropertyName("Evasion")]
    public int? Evasion { get; set; }

    public int? EvasionMin => Evasion;
    public int? EvasionMax => Evasion;

    [JsonPropertyName("EnergyShield")]
    public int? EnergyShield { get; set; }

    public int? EnergyShieldMin => EnergyShield;
    public int? EnergyShieldMax => EnergyShield;

    [JsonPropertyName("Ward")]
    public int? Ward { get; set; }

    public int? WardMin => Ward;
    public int? WardMax => Ward;
}