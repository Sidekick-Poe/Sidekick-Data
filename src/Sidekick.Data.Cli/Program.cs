using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Sidekick.Common;
using Sidekick.Data.Builder;
using Sidekick.Data.Builder.ItemClasses;
using Sidekick.Data.Builder.ItemDefinitions;
using Sidekick.Data.Builder.Leagues;
using Sidekick.Data.Builder.Ninja;
using Sidekick.Data.Builder.Pseudo;
using Sidekick.Data.Builder.Repoe;
using Sidekick.Data.Builder.Stats;
using Sidekick.Data.Builder.StatsInvariant;
using Sidekick.Data.Builder.Trade;
using Sidekick.Data.Languages;

#region Services

var services = new ServiceCollection();
services.AddLogging(o =>
{
    o.SetMinimumLevel(LogLevel.Trace);
    o.AddFilter("Microsoft", LogLevel.Warning);
    o.AddFilter("System", LogLevel.Warning);
    o.AddConsole();
});

services.AddSidekickCommon(SidekickApplicationType.DataBuilder);

services.TryAddSingleton<LeagueBuilder>();
services.TryAddSingleton<NinjaDownloader>();
services.TryAddSingleton<PseudoBuilder>();
services.TryAddSingleton<RepoeDownloader>();
services.TryAddSingleton<StatBuilder>();
services.TryAddSingleton<TradeDownloader>();
services.TryAddSingleton<TradeStatBuilder>();
services.TryAddSingleton<ItemClassBuilder>();
services.TryAddSingleton<ItemDefinitionBuilder>();
services.TryAddSingleton<StatsInvariantBuilder>();
services.TryAddSingleton<TradeFilterBuilder>();
services.TryAddSingleton<RawDataProvider>();
services.TryAddSingleton<DataBuilder>();

var serviceProvider = services.BuildServiceProvider();
var dataBuilder = serviceProvider.GetRequiredService<DataBuilder>();
var gameLanguageProvider = serviceProvider.GetRequiredService<IGameLanguageProvider>();

#endregion

#region Configuration

var runLanguage = string.Empty;

var download = false;
var build = true;

var runItems = false;
var runStats = false;
var runTrade = false;
var runRepoe = false;
var runPseudo = false;
var runNinja = false;

for (var i = 0; i < args.Length; i++)
{
    var a = args[i];
    switch (a)
    {
        case "--language" when i + 1 < args.Length:
            runLanguage = args[++i];
            break;
        case "--items":
            runItems = true;
            break;
        case "--stats":
            runStats = true;
            break;
        case "--trade":
            runTrade = true;
            break;
        case "--repoe":
            runRepoe = true;
            break;
        case "--pseudo":
            runPseudo = true;
            break;
        case "--ninja":
            runNinja = true;
            break;
        case "--download":
            download = true;
            break;
    }
}

#endregion

if (!runItems && !runStats && !runTrade && !runRepoe && !runPseudo && !runNinja)
{
    runItems = true;
    runStats = true;
    runTrade = true;
    runRepoe = true;
    runPseudo = true;
    runNinja = true;
}

var languages = gameLanguageProvider.GetList();
if (!string.IsNullOrEmpty(runLanguage))
{
    languages = languages.Where(x => x.Code == runLanguage).ToList();
}

foreach (var language in languages)
{
    if (download)
    {
        await dataBuilder.DownloadRawFiles(language,
                                           trade: runTrade,
                                           repoe: runRepoe,
                                           ninja: runNinja);
    }

    if (build)
    {
        await dataBuilder.BuildDataFiles(language,
                                         items: runItems,
                                         stats: runStats,
                                         trade: runTrade,
                                         pseudo: runPseudo);
    }
}
