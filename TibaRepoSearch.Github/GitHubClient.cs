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

    public async Task<GitHubRepositoryMetadata?> FetchRepositoryMetadataAsync(string repoId)
    {
        try
        {
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(_options.RetryCount, retryAttempt => TimeSpan.FromSeconds(_options.DelaySeconds));

            var repoResponse = await retryPolicy.ExecuteAsync(async () =>
                await _httpClient.GetFromJsonAsync<GitHubRepoResponse>($"repositories/{repoId}"));

            if (repoResponse == null) return null;

            var readmeSize = await GetReadmeSizeAsync(repoResponse.full_name);

            var result = new GitHubRepositoryMetadata(
                repoResponse.license,
                repoResponse.topics ?? Array.Empty<string>(),
                repoResponse.language,
                readmeSize,
                repoResponse.open_issues_count,
                repoResponse.forks_count,
                repoResponse.stargazers_count,
                repoResponse.pushed_at,
                repoResponse.default_branch
            );

            _logger.LogTrace("[GitHubClient.FetchRepositoryMetadataAsync] {repoId} OK", repoId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[GitHubClient.FetchRepositoryMetadataAsync] {repoId} {Message}", repoId, ex.Message);
            return null;
        }
    }

    private async Task<int> GetReadmeSizeAsync(string fullName)
    {
        try
        {
            var readmeResponse = await _httpClient.GetFromJsonAsync<GitHubReadmeResponse>($"repos/{fullName}/readme");
            return readmeResponse?.size ?? 0;
        }
        catch
        {
            return 0;
        }
    }
    
    private record GitHubRepoResponse(string full_name, GitHubLicense? license, string[] topics, string? language, int open_issues_count, int forks_count, int stargazers_count, DateTime pushed_at, string default_branch);
    private record GitHubReadmeResponse(int size);
}