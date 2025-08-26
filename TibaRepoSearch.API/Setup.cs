
namespace TibaRepoSearch;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddTransient<IRepositorySearchUseCase>(_ => new RepositorySearchUseCase());
        services.AddTransient<IAddToFavoritesUseCase>(_ => new AddToFavoritesUseCase());
        services.AddTransient<IFetchRepositoryAnalysisUseCase>(_ => new FetchRepositoryAnalysisUseCase());
        services.AddTransient<IListUserFavoritesUseCase>(_ => new ListUserFavoritesUseCase());
        services.AddTransient<IRemoveUserFavoriteUseCase>(_ => new RemoveUserFavoriteUseCase());
        return services;
    }
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure layer services here, using configuration if needed
        return services;
    }
}

internal class RepositorySearchUseCase : IRepositorySearchUseCase
{
    public Task<IEnumerable<Repository>> SearchAsync(string query)
    {
        var repositories = new List<Repository>
        {
            new Repository("angular", "angular", 95000, DateTime.UtcNow.AddDays(-1), "The modern web developer's platform", "1")
        };
        return Task.FromResult(repositories.Where(r => r.Name.Contains(query, StringComparison.OrdinalIgnoreCase)));
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