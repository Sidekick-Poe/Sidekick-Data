using Microsoft.Extensions.Logging;
using Sidekick.Data.Ninja;

namespace Sidekick.Data;

internal sealed class CommandExecutor(ILogger<CommandExecutor> logger,
    NinjaDownloader ninjaDownloader)
{
    public async Task<int> Execute(string[] args)
    {
        try
        {
            var options = ParseOptions(args);

            // await TradeDownloader.DownloadAll(options);
            await ninjaDownloader.DownloadAll(options);

            Console.WriteLine("Done.");
            return 0;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Failed to execute command.");
            return 2;
        }
    }

    private CommandOptions ParseOptions(string[] args)
    {
        var opt = new CommandOptions();
        for (var i = 0; i < args.Length; i++)
        {
            var a = args[i];
            switch (a)
            {
                case "--folder" when i + 1 < args.Length:
                    opt.DataFolder = args[++i];
                    break;
                case "--poe1" when i + 1 < args.Length:
                    opt.Poe1League = args[++i];
                    break;
                case "--poe2" when i + 1 < args.Length:
                    opt.Poe2League = args[++i];
                    break;
                case "--languages" when i + 1 < args.Length:
                    opt.LanguageCodes = args[++i].Split(',').Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
                    break;
                case "--paths" when i + 1 < args.Length:
                    opt.TradePaths = args[++i].Split(',').Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
                    break;
                case "--timeout" when i + 1 < args.Length && int.TryParse(args[++i], out var t):
                    opt.TimeoutSeconds = t;
                    break;
                case "--help":
                    PrintHelp();
                    break;
            }
        }

        return opt;
    }

    private void PrintHelp()
    {
        logger.LogInformation(@"Sidekick.Data - Console downloader (decoupled)

Usage:
  download-trade --folder <PATH> [--languages en,de,es] [--paths items,stats,static,filters]
  analyze --folder <PATH>
  download-ninja --folder <PATH> [--poe1 <LEAGUE>] [--poe2 <LEAGUE>]

Notes:
  - Trade data downloads for both games (poe1, poe2). Also downloads 'leagues' in English.
  - Ninja data requires league names; pass one or both of --poe1/--poe2.
");
    }
}