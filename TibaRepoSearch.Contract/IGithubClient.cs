namespace TibaRepoSearch.Contract;

public interface IGithubClient
{
    Task<GitHubSearchResponse> SearchRepositoriesAsync(string query);
}