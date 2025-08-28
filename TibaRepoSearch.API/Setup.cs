
using TibaRepoSearch.Contract;

namespace TibaRepoSearch;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddTransient<IRepositorySearchUseCase, RepositorySearchUseCase>();
        services.AddTransient<IAddToFavoritesUseCase>(_ => new AddToFavoritesUseCase());
        services.AddTransient<IFetchRepositoryAnalysisUseCase>(_ => new FetchRepositoryAnalysisUseCase());
        services.AddTransient<IListUserFavoritesUseCase>(_ => new ListUserFavoritesUseCase());
        services.AddTransient<IRemoveUserFavoriteUseCase>(_ => new RemoveUserFavoriteUseCase());
        return services;
    }
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddHttpClient<IGithubClient, GitHubClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "TibaRepoSearch");
        });
        return services;
    }
}

internal class RepositorySearchUseCase : IRepositorySearchUseCase
{
    private readonly IGithubClient _githubClient;

    public RepositorySearchUseCase(IGithubClient githubClient)
    {
        _githubClient = githubClient;
    }

    public async Task<IEnumerable<Repository>> SearchAsync(string query)
    {
        var response = await _githubClient.SearchRepositoriesAsync(query);
        return response.Items.Select(repo => new Repository(
            repo.Name,
            repo.Owner.Login,
            repo.StargazersCount,
            repo.UpdatedAt,
            repo.Description ?? string.Empty,
            repo.Id.ToString()));
    }
}

internal class AddToFavoritesUseCase : IAddToFavoritesUseCase
{
    public Task AddAsync(AddFavoriteRequest request, string userId)
    {
        return Task.CompletedTask;
    }
}

internal class FetchRepositoryAnalysisUseCase : IFetchRepositoryAnalysisUseCase
{
    public Task<Analysis?> FetchAnalysisAsync(string repoId)
    {
        return Task.FromResult<Analysis?>(null);
    }
}

internal class ListUserFavoritesUseCase : IListUserFavoritesUseCase
{
    public Task<IEnumerable<FavoriteRepository>> ListAsync(string userId)
    {
        return Task.FromResult(Enumerable.Empty<FavoriteRepository>());
    }
}

internal class RemoveUserFavoriteUseCase : IRemoveUserFavoriteUseCase
{
    public Task RemoveAsync(string repoId, string userId)
    {
        return Task.CompletedTask;
    }
}

internal class GitHubClient : IGithubClient
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