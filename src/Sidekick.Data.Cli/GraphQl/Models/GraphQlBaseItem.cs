using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.GraphQl.Models;

public class GraphQlBaseItem
{
    [JsonPropertyName("_index")] public int Index { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("itemClassesKey")] public int? ItemClassesKey { get; set; }
    [JsonPropertyName("itemClass")] public int? ItemClass { get; set; }
    [JsonPropertyName("dropLevel")] public int? DropLevel { get; set; }
}

public class GraphQlArmourType
{
    [JsonPropertyName("baseItemTypesKey")] public int BaseItemTypesKey { get; set; }
    [JsonPropertyName("armourMin")] public int? ArmourMin { get; set; }
    [JsonPropertyName("armourMax")] public int? ArmourMax { get; set; }
    [JsonPropertyName("evasionMin")] public int? EvasionMin { get; set; }
    [JsonPropertyName("evasionMax")] public int? EvasionMax { get; set; }
    [JsonPropertyName("energyShieldMin")] public int? EnergyShieldMin { get; set; }
    [JsonPropertyName("energyShieldMax")] public int? EnergyShieldMax { get; set; }
    [JsonPropertyName("wardMin")] public int? WardMin { get; set; }
    [JsonPropertyName("wardMax")] public int? WardMax { get; set; }
}

public class BaseItemTypesResponse
{
    [JsonPropertyName("poe1_baseItemTypes")] public List<GraphQlBaseItem>? Poe1BaseItemTypes { get; set; }
    [JsonPropertyName("poe2_baseItemTypes")] public List<GraphQlBaseItem>? Poe2BaseItemTypes { get; set; }
}

public class ArmourTypesResponse
{
    [JsonPropertyName("poe1_armourTypes")] public List<GraphQlArmourType>? Poe1ArmourTypes { get; set; }
    [JsonPropertyName("poe2_armourTypes")] public List<GraphQlArmourType>? Poe2ArmourTypes { get; set; }
}
