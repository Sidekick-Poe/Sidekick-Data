using System.Text.Json.Serialization;

namespace Sidekick.Data.Cli.ItemClasses.Responses;

public class GraphQlItemClass
{
    public static string GetQuery(GameType game, string language) {
        if (game == GameType.PathOfExile1)
        {
            return $@"
              query {{
                poe1_itemClasses(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        if (game == GameType.PathOfExile2)
        {
            return $@"
              query {{
                poe2_itemClasses(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        throw new Exception($"[ArmourType] Unknown game: {game}");
    }

    public static string GetQueryProperties(GameType game) {
        return $@"
          Id
          Name";
    }

    [JsonPropertyName("Id")]
    public string? Id { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }
}