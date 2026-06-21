using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sidekick.Common.Folder;

public class FolderProvider(
    IOptions<SidekickConfiguration> configuration,
    ILogger<FolderProvider> logger)
{
    public void OpenFolder(string path)
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            };
            process.Start();
        }
        catch
        {
            logger.LogError("[Folder] Failed to open data file path.");
        }
    }

    /// <summary>
    /// Gets the folder where the data files are stored.
    ///
    /// <para>Windows: C:\Users\___\AppData\Roaming</para>
    /// <para>Linux: /home/___/.config</para>
    /// <para>OSX: /Users/___/.config</para>
    /// </summary>
    public string GetUserDataPath(string path = "")
    {
        var environmentFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var sidekickFolder = Path.Combine(environmentFolder, "sidekick");

        if (!Directory.Exists(sidekickFolder))
        {
            Directory.CreateDirectory(sidekickFolder);
        }

        return !string.IsNullOrEmpty(path) ? Path.Combine(sidekickFolder, path) : sidekickFolder;
    }

    public string GetDataDirectory()
    {
        if (Debugger.IsAttached || configuration.Value.ApplicationType == SidekickApplicationType.DataBuilder || configuration.Value.ApplicationType == SidekickApplicationType.Test)
        {
            var solutionDirectory = FindSolutionDirectory();
            if (!string.IsNullOrEmpty(solutionDirectory))
            {
                return $"{solutionDirectory}/data";
            }
        }

        return Path.Combine(AppContext.BaseDirectory, "wwwroot/data");
    }

    public string? FindSolutionDirectory(string? startDirectory = null)
    {
        var dir = new DirectoryInfo(startDirectory ?? AppContext.BaseDirectory);

        while (dir != null)
        {
            var sln = dir.EnumerateFiles("*.sln", SearchOption.TopDirectoryOnly)
                .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();

            if (sln != null)
                return sln.DirectoryName;

            dir = dir.Parent;
        }

        return null;
    }
}
