using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.Uniques.Responses;

public class GraphQlWords
{
    public static string GetQueryProperties(GameType game) {
        return $@"
            Text
            Text2";
    }

    [JsonPropertyName("Text")]
    public string? Id { get; set; }

    [JsonPropertyName("Text2")]
    public string? Text { get; set; }
}