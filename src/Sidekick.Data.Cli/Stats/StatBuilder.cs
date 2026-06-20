using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sidekick.Common;
using Sidekick.Data.Cli.GraphQl;
using Sidekick.Data.Cli.GraphQl.Models;
using Sidekick.Data.Cli.Stats.Hooks;
using Sidekick.Data.Fuzzy;
using Sidekick.Data.Languages;
using Sidekick.Data.Stats;
using Sidekick.Data.StatsInvariant;
using Sidekick.Data.Trade;

namespace Sidekick.Data.Cli.Stats;

public class StatBuilder(
    ILogger<StatBuilder> logger,
    IOptions<SidekickConfiguration> configuration,
    DataProvider dataProvider,
    GraphQlClient graphQlClient,
    IFuzzyService fuzzyService)
{
    public async Task Build(IGameLanguage language)
    {
        try
        {
            var patterns = new StatPatternBuilder(language, fuzzyService);
            await Build(GameType.PathOfExile1, language, patterns);
            await Build(GameType.PathOfExile2, language, patterns);
        }
        catch (Exception ex)
        {
            if (configuration.Value.ApplicationType == SidekickApplicationType.DataBuilder || configuration.Value.ApplicationType == SidekickApplicationType.Test)
                throw;
            logger.LogError(ex, $"Failed to build stats for {language.Code}.");
        }
    }

    private async Task Build(GameType game, IGameLanguage language, StatPatternBuilder patternBuilder)
    {
        var statDescriptions = await LoadStatDescriptions(game, language);
        var context = new StatBuilderContext(
            Game: game, Language: language, PatternBuilder: patternBuilder,
            GraphQlStatDescriptions: statDescriptions,
            TradeDefinitions: await dataProvider.Read<List<TradeStatDefinition>>(game, DataType.TradeStats, language),
            InvariantDetails: await dataProvider.Read<StatsInvariantDetails>(game, DataType.StatsInvariant));

        List<BaseHook> hooks = [new IgnoreGameIds(), new LogbookBosses(context), new LogbookFactions(context), new IncursionRooms(context)];
        var definitions = BuildGameStats(context).ToList();
        foreach (var hook in hooks) hook.OnAfterGameBuild(definitions);
        definitions.AddRange(BuildMissingTradeStats(context, definitions));
        foreach (var hook in hooks) hook.OnAfterTradeBuild(definitions);
        await dataProvider.Write(game, DataType.Stats, language, definitions);
    }

    private async Task<List<GraphQlStatDescription>> LoadStatDescriptions(GameType game, IGameLanguage language)
    {
        var lang = GetLanguageName(language.Code);
        var gameNum = game == GameType.PathOfExile1 ? 1 : 2;
        var path = game == GameType.PathOfExile1 ? "stat_descriptions.txt" : "stat_descriptions.csd";
        var query = $"query {{ statDescriptions(game: {gameNum}, path: \"{path}\", lang: \"{lang}\") {{ statIds rules {{ conditionTexts template negate }} }} }}";
        var result = await graphQlClient.QueryAsync<StatDescriptionsResponse>(query);
        return result?.StatDescriptions ?? new List<GraphQlStatDescription>();
    }

    private IEnumerable<StatDefinition> BuildGameStats(StatBuilderContext context)
    {
        foreach (var graphStat in context.GraphQlStatDescriptions)
        {
            if (graphStat.StatIds == null || graphStat.Rules == null) continue;
            foreach (var rule in graphStat.Rules)
            {
                if (string.IsNullOrEmpty(rule.Template)) continue;
                var text = context.PatternBuilder.GetText(rule.Template);
                var value = GetValue(rule);
                yield return new StatDefinition
                {
                    GameIds = graphStat.StatIds,
                    Source = DataSource.Game,
                    Text = text,
                    Negate = rule.Negate,
                    Pattern = context.PatternBuilder.GetPattern(rule.Template),
                    Value = value,
                    Lines = text.Split('\n').Length,
                };
            }
        }
        yield break;

        int? GetValue(GraphQlStatDescriptionRule rule)
        {
            if (rule.ConditionTexts == null || rule.ConditionTexts.Count != 1) return null;
            var parts = rule.ConditionTexts[0].Split(' ');
            if (parts.Length == 1 && int.TryParse(parts[0], out var minVal)) return minVal;
            return null;
        }
    }

    private IEnumerable<StatDefinition> BuildMissingTradeStats(StatBuilderContext context, List<StatDefinition> definitions)
    {
        var definedTradeStats = definitions.Where(x => x.TradeIds != null).SelectMany(x => x.TradeIds!).ToHashSet();
        foreach (var tradeDefinition in context.TradeDefinitions)
        {
            if (definedTradeStats.Contains(tradeDefinition.Id)) continue;
            var text = tradeDefinition.Text ?? string.Empty;
            yield return new StatDefinition
            {
                Source = DataSource.Trade,
                TradeIds = new List<string> { tradeDefinition.Id },
                Text = text,
                Pattern = new Regex(Regex.Escape(text)),
                Lines = text.Split('\n').Length,
            };
        }
    }

    private static string GetLanguageName(string code) => code switch
    {
        "en" => "English", "de" => "German", "es" => "Spanish", "fr" => "French",
        "ja" => "Japanese", "ko" => "Korean", "pt" => "Portuguese", "ru" => "Russian",
        "th" => "Thai", "zh" => "Traditional Chinese", _ => "English",
    };
}
