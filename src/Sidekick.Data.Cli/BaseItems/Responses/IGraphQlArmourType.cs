namespace Sidekick.Data.Cli.BaseItems.Responses;

public interface IGraphQlArmourType
{
    static string GetQuery(GameType game, string language) {
        if (game == GameType.PathOfExile1)
        {
            return $@"
              query {{
                poe1_armourTypes(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        if (game == GameType.PathOfExile2)
        {
            return $@"
              query {{
                poe2_armourTypes(lang: ""{language}"") {{
                  {GetQueryProperties(game)}
                }}
              }}";
        }

        throw new Exception($"[ArmourType] Unknown game: {game}");
    }

    static string GetQueryProperties(GameType game) {
        if (game == GameType.PathOfExile1)
        {
            return $@"
              ArmourMax
              ArmourMin
              EnergyShieldMax
              EnergyShieldMin
              EvasionMax
              EvasionMin
              WardMax
              WardMin
              BaseItemTypesKey {{
                {GraphQlBaseItem.GetQueryProperties(game)}
              }}";
        }

        if (game == GameType.PathOfExile2)
        {
            return $@"
              Armour
              EnergyShield
              Evasion
              Ward
              BaseItemType {{
                {GraphQlBaseItem.GetQueryProperties(game)}
              }}";
        }

        throw new Exception($"[ArmourType] Unknown game: {game}");
    }

    GraphQlBaseItem BaseItem { get; }
    int? ArmourMin { get; }
    int? ArmourMax { get; }
    int? EvasionMin { get; }
    int? EvasionMax { get; }
    int? EnergyShieldMin { get; }
    int? EnergyShieldMax { get; }
    int? WardMin { get; }
    int? WardMax { get; }
}