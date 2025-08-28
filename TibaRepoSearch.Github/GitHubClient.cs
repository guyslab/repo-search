using System.Net.Http.Json;

namespace TibaRepoSearch;

public class GitHubClient : IGithubClient
{
    private readonly HttpClient _httpClient;

    public GitHubClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GitHubSearchResponse> SearchRepositoriesAsync(string query)
    {
        var response = await _httpClient.GetFromJsonAsync<GitHubSearchResponse>($"search/repositories?q={Uri.EscapeDataString(query)}");
        return response ?? new GitHubSearchResponse(0, false, Array.Empty<GitHubRepository>());
    }
}