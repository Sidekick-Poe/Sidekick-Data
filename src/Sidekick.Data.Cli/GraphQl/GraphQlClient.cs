using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Sidekick.Data.Cli.GraphQl;

public class GraphQlClient(ILogger<GraphQlClient> logger)
{
    private const int Port = 4000;
    private static string GraphQlUrl => $"http://127.0.0.1:{Port}/graphql";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };


    public async Task<T?> QueryAsync<T>(string query, object? variables = null) where T : class
    {
        var requestBody = new Dictionary<string, object> { ["query"] = query };
        if (variables != null) requestBody["variables"] = variables;

        var json = JsonSerializer.Serialize(requestBody, JsonOptions);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            var response = await httpClient.PostAsync(GraphQlUrl, httpContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GraphQlResponse<T>>(responseJson, JsonOptions);

            if (result?.Errors?.Length > 0)
                foreach (var error in result.Errors)
                    logger.LogError("[GraphQL] {Message}", error.Message);

            return result?.Data;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "[GraphQL] Failed to query {Url}. Is the GraphQL API running?", GraphQlUrl);
            throw new InvalidOperationException(
                $"Failed to query the Sidekick GraphQL API at {GraphQlUrl}. " +
                "Ensure the GraphQL API is running before building data.", ex);
        }
    }

    private sealed class GraphQlResponse<T> where T : class
    {
        public T? Data { get; set; }
        public GraphQlError[]? Errors { get; set; }
    }

    private sealed class GraphQlError
    {
        public string Message { get; set; } = string.Empty;
    }
}
