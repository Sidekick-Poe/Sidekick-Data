using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlItemVisualIdentity
{
    public static string GetQueryProperties(GameType game) {
        return $@"
          Id
          DDSFile";
    }

    [JsonPropertyName("Id")]
    public required string Id { get; set; }

    [JsonPropertyName("DDSFile")]
    public string? Image { get; set; }
}