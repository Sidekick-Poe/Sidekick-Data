using System.Diagnostics;

namespace Sidekick.Common;

public static class SidekickPaths
{
    public static void OpenFolder(string path)
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

    /// <summary>
    /// Gets the folder where the data files are stored.
    ///
    /// <para>Windows: C:\Users\___\AppData\Roaming</para>
    /// <para>Linux: /home/___/.config</para>
    /// <para>OSX: /Users/___/.config</para>
    /// </summary>
    public static string GetUserDataPath(string path = "")
    {
        var environmentFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var sidekickFolder = Path.Combine(environmentFolder, "sidekick");

        if (!Directory.Exists(sidekickFolder))
        {
            Directory.CreateDirectory(sidekickFolder);
        }

        return !string.IsNullOrEmpty(path) ? Path.Combine(sidekickFolder, path) : sidekickFolder;
    }

    public static string GetDataDirectory()
    {
        var solutionDirectory = FindSolutionDirectory();
        if (!string.IsNullOrEmpty(solutionDirectory))
        {
            return $"{solutionDirectory}/data";
        }

        return Path.Combine(AppContext.BaseDirectory, "data");
    }

    public static string? FindSolutionDirectory(string? startDirectory = null)
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