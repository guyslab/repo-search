using StackExchange.Redis;

namespace TibaRepoSearch;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RepositorySearchOptions>(options => { options.CacheTtlSeconds = 60; });
        services.AddSingleton<IRepositorySearchUseCase, RepositorySearchUseCase>();
        services.AddSingleton<IAddToFavoritesUseCase, AddToFavoritesUseCase>();
        services.AddSingleton<IFetchRepositoryAnalysisUseCase, FetchRepositoryAnalysisUseCase>();
        services.AddSingleton<IListUserFavoritesUseCase, ListUserFavoritesUseCase>();
        services.AddSingleton<IRemoveUserFavoriteUseCase, RemoveUserFavoriteUseCase>();
        services.AddHttpContextAccessor();
        services.AddScoped<ApiManagement.IRequestContext, ApiManagement.RequestContext>();
        return services;
    }
    
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GithubClientOptions>(options => { options.RetryCount = 3; options.DelaySeconds = 1; });
        services.AddHttpClient<IGithubClient, GitHubClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "TibaRepoSearch");
        });

        services.AddSingleton<IConnectionMultiplexer>(provider =>
            ConnectionMultiplexer.Connect("localhost:6379"));
        services.AddSingleton<IDatabase>(provider =>
            provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
        services.AddSingleton<ICacheThrough, RedisCacheThrough>();
        
        return services;
    }
}