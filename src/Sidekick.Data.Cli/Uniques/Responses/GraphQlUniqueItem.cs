using System.Text.Json.Serialization;
using Sidekick.Data.Cli.BaseItems.Responses;

namespace Sidekick.Data.Cli.Uniques.Responses;

public class GraphQlUniqueItem
{
    public static string GetQuery(GameType game, string language)
    {
        if (game == GameType.PathOfExile1)
        {
            return $@"
              query {{
                poe1_uniqueStashLayout(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        if (game == GameType.PathOfExile2)
        {
            return $@"
              query {{
                poe2_uniqueStashLayout(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        throw new Exception($"[BaseItem] Unknown game: {game}");
    }

    public static string GetQueryProperties(GameType game) {
        return $@"
          WordsKey {{
            {GraphQlWords.GetQueryProperties(game)}
          }}
          ItemVisualIdentityKey {{
            {GraphQlItemVisualIdentity.GetQueryProperties(game)}
          }}";
    }

    [JsonPropertyName("ItemVisualIdentityKey")]
    public GraphQlItemVisualIdentity? ItemVisualIdentity { get; set; }

    [JsonPropertyName("WordsKey")]
    public GraphQlWords? Words { get; set; }
}