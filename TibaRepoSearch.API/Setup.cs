using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace TibaRepoSearch;

public static class ServiceCollectionExtensions
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
        services.AddScoped<IRequestContext, RequestContext>();
        services.AddSingleton<IAddOrUpdateFavoriteRepositoryCommandFactory, AddOrUpdateFavoriteRepositoryCommandFactory>();
        services.AddSingleton<IListFavoriteRepositoryWithAnalysisCommandFactory, ListFavoriteRepositoryWithAnalysisCommandFactory>();
        services.AddSingleton<IRemoveFavoriteRepositoryWithAnalysisCommandFactory, RemoveFavoriteRepositoryWithAnalysisCommandFactory>();
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

        var dbHost = configuration["Database:Host"] ?? "localhost";
        var dbName = configuration["Database:Name"] ?? "tibarepodb";
        var dbUser = configuration["Database:User"] ?? "tibauser";
        var dbPassword = configuration["Database:Password"] ?? "tibapass";
        var connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword}";
        
        services.AddDbContext<FavoriteRepositoriesContext>(options =>
            options.UseNpgsql(connectionString));        

        var redisConnectionString = configuration["Redis:ConnectionString"] ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(provider =>
            ConnectionMultiplexer.Connect(redisConnectionString));
        services.AddSingleton<IDatabase>(provider =>
            provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
        services.AddSingleton<ICacheThrough, RedisCacheThrough>();
        
        return services;
    }
}