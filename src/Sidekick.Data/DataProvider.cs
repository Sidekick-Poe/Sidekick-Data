using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Sidekick.Common.Enums;
using Sidekick.Common.Exceptions;
using Sidekick.Common.Folder;
using Sidekick.Data.Languages;

namespace Sidekick.Data;

public class DataProvider(ILogger<DataProvider> logger, FolderProvider folderProvider)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.Preserve,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
    };

    public string DataDirectory { get; } = folderProvider.GetDataDirectory();

    public Task Write(GameType game, DataType type, IGameLanguage language, object data)
    {
        return Write(GetFilePath(game, type, language.Code), data);
    }

    public Task Write(GameType game, DataType type, IGameLanguage language, Stream stream)
    {
        return Write(GetFilePath(game, type, language.Code), stream);
    }

    public Task Write(GameType game, DataType type, object data)
    {
        return Write(GetFilePath(game, type, "invariant"), data);
    }

    private async Task Write(string filePath, object data)
    {
        Directory.CreateDirectory(DataDirectory);

        var path = Path.Combine(DataDirectory, filePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(fs, data, JsonOptions);
        logger.LogInformation($"Saved {path}");
    }

    private async Task Write(string filePath, Stream stream)
    {
        Directory.CreateDirectory(DataDirectory);

        var path = Path.Combine(DataDirectory, filePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fs);
        logger.LogInformation($"Saved {path}");
    }

    public Task<TResult> Read<TResult>(GameType game, DataType type, IGameLanguage language)
        where TResult : class
    {
        return Read<TResult>(GetFilePath(game, type, language.Code));
    }

    public Task<TResult> Read<TResult>(GameType game, DataType type)
        where TResult : class
    {
        return Read<TResult>(GetFilePath(game, type, "invariant"));
    }

    private async Task<TResult> Read<TResult>(string filePath)
        where TResult : class
    {
        Directory.CreateDirectory(DataDirectory);

        var path = Path.Combine(DataDirectory, filePath);
        if (!File.Exists(path)) throw new SidekickException($"The data file does not exist. {filePath}");

        await using var fileStream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<TResult>(fileStream, JsonOptions)
               ?? throw new SidekickException("The data file could not be read successfully.");
    }

    private string GetFilePath(GameType game, DataType type, string languageCode)
    {
        return game.GetValueAttribute() + "/" + string.Format(type.GetValueAttribute(), languageCode);
    }
}
