using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Polly;

namespace TibaRepoSearch;

public class GitHubClient : IGithubClient
{
    private readonly HttpClient _httpClient;
    private readonly GithubClientOptions _options;
    private readonly ILogger<GitHubClient> _logger;

    public GitHubClient(HttpClient httpClient, IOptions<GithubClientOptions> options, ILogger<GitHubClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        _logger.LogTrace("[GitHubClient..ctor] {httpClient};{options} OK", httpClient, options);
    }

    public async Task<GitHubSearchResponse> SearchRepositoriesAsync(string query)
    {
        try
        {
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(_options.RetryCount, retryAttempt => TimeSpan.FromSeconds(_options.DelaySeconds));

            var response = await retryPolicy.ExecuteAsync(async () =>
                await _httpClient.GetFromJsonAsync<GitHubSearchResponse>($"search/repositories?q={Uri.EscapeDataString(query)}"));
            
            var result = response ?? new GitHubSearchResponse(0, false, Array.Empty<GitHubRepository>());
            _logger.LogTrace("[GitHubClient.SearchRepositoriesAsync] {query} OK", query);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[GitHubClient.SearchRepositoriesAsync] {query} {Message}", query, ex.Message);
            throw;
        }
    }
}