namespace Sidekick.Data.Cli.BaseItems.Responses;

public interface IGraphQlAttributeRequirement
{
    static string GetQuery(GameType game, string language) {
        if (game == GameType.PathOfExile1)
        {
            return $@"
              query {{
                poe1_componentAttributeRequirements(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        if (game == GameType.PathOfExile2)
        {
            return $@"
              query {{
                poe2_attributeRequirements(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        throw new Exception($"[AttributeRequirement] Unknown game: {game}");
    }

    static string GetQueryProperties(GameType game) {
        if (game == GameType.PathOfExile1)
        {
            return @"
              BaseItemTypesKey
              ReqDex
              ReqInt
              ReqStr";
        }

        if (game == GameType.PathOfExile2)
        {
            return $@"
              ReqDex
              ReqInt
              ReqStr
              BaseItemType {{
                {GraphQlBaseItem.GetQueryProperties(game)}
              }}";
        }

        throw new Exception($"[AttributeRequirement] Unknown game: {game}");
    }

    string BaseItemId { get; }
    int RequiresDexterity { get; }
    int RequiresIntelligence { get; }
    int RequiresStrength { get; }
}