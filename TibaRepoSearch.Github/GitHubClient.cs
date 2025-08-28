using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Polly;

namespace TibaRepoSearch;

public class GitHubClient : IGithubClient
{
    private readonly HttpClient _httpClient;
    private readonly GithubClientOptions _options;

    public GitHubClient(HttpClient httpClient, IOptions<GithubClientOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<GitHubSearchResponse> SearchRepositoriesAsync(string query)
    {
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(_options.RetryCount, retryAttempt => TimeSpan.FromSeconds(_options.DelaySeconds));

        var response = await retryPolicy.ExecuteAsync(async () =>
            await _httpClient.GetFromJsonAsync<GitHubSearchResponse>($"search/repositories?q={Uri.EscapeDataString(query)}"));
        
        return response ?? new GitHubSearchResponse(0, false, Array.Empty<GitHubRepository>());
    }
}