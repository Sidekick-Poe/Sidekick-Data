using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.GraphQl.Models;

public class GraphQlStatDescription
{
    [JsonPropertyName("statIds")] public List<string>? StatIds { get; set; }
    [JsonPropertyName("rules")] public List<GraphQlStatDescriptionRule>? Rules { get; set; }
}

public class GraphQlStatDescriptionRule
{
    [JsonPropertyName("conditionTexts")] public List<string>? ConditionTexts { get; set; }
    [JsonPropertyName("template")] public string? Template { get; set; }
    [JsonPropertyName("negate")] public bool Negate { get; set; }
}

public class StatDescriptionsResponse
{
    [JsonPropertyName("statDescriptions")] public List<GraphQlStatDescription>? StatDescriptions { get; set; }
}
