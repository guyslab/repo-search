namespace TibaRepoSearch;

public interface IGithubClient
{
    Task<GitHubSearchResponse> SearchRepositoriesAsync(string query);
}