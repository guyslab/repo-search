namespace TibaRepoSearch;

public interface IGithubClient
{
    Task<GitHubSearchResponse> SearchRepositoriesAsync(string query);
    Task<GitHubRepositoryMetadata?> FetchRepositoryMetadataAsync(string repoId);
}