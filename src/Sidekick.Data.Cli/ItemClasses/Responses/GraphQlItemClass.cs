using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.ItemClasses.Responses;

public class GraphQlItemClass
{
    [JsonPropertyName("Id")] public string? Id { get; set; }
    [JsonPropertyName("Name")] public string? Name { get; set; }
}