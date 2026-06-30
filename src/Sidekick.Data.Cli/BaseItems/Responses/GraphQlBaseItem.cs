using System.Text.Json.Serialization;
using Sidekick.Data.Cli.ItemClasses.Responses;

namespace Sidekick.Data.Cli.BaseItems.Responses;

public class GraphQlBaseItem
{
    public static string GetQuery(GameType game, string language)
    {
        if (game == GameType.PathOfExile1)
        {
            return $@"
              query {{
                poe1_baseItemTypes(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        if (game == GameType.PathOfExile2)
        {
            return $@"
              query {{
                poe2_baseItemTypes(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        throw new Exception($"[BaseItem] Unknown game: {game}");
    }

    public static string GetQueryProperties(GameType game) {
        var itemClassProperty = game == GameType.PathOfExile1 ? "ItemClassesKey" : "ItemClass";

        return $@"
          Id
          Name
          DropLevel
          {itemClassProperty} {{
            {GraphQlItemClass.GetQueryProperties(game)}
          }}
          ItemVisualIdentity {{
            {GraphQlItemVisualIdentity.GetQueryProperties(game)}
          }}";
    }

    [JsonPropertyName("Id")]
    public required string Id { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("ItemClassesKey")]
    public GraphQlItemClass? ItemClass { get; set; }

    [JsonPropertyName("DropLevel")]
    public int? DropLevel { get; set; }

    [JsonPropertyName("ItemVisualIdentity")]
    public GraphQlItemVisualIdentity? ItemVisualIdentity { get; set; }
}