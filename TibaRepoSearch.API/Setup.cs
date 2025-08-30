using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace TibaRepoSearch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        try
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
            services.AddSingleton<IAddOrUpdateFavoriteRepositoryAnalysisCommandFactory, AddOrUpdateFavoriteRepositoryAnalysisCommandFactory>();
            Console.WriteLine($"[ServiceCollectionExtensions.AddApplicationLayer] {services};{configuration} OK");
            return services;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ServiceCollectionExtensions.AddApplicationLayer] {services};{configuration} {ex.Message}");
            throw;
        }
    }
    
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        try
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
            
            services.AddDbContextFactory<FavoriteRepositoriesContext>(options =>
                options.UseNpgsql(connectionString));

            var redisConnectionString = configuration["Redis:ConnectionString"] ?? "localhost:6379";
            services.AddSingleton<Lazy<IConnectionMultiplexer>>(provider =>
                new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisConnectionString)));
            services.AddSingleton<ICacheThrough, RedisCacheThrough>();
            
            var rabbitMQHost = configuration["RabbitMQ:Host"] ?? "localhost";
            var repoFavoritedQueue = configuration["RabbitMQ:RepoFavoritedQueue"] ?? "repo.favorited";
            var rabbitMQUser = configuration["RabbitMQ:UserName"] ?? "admin";
            var rabbitMQPassword = configuration["RabbitMQ:Password"] ?? "admin";
            services.AddSingleton<IEventPublisher<RepositoryFavoritedMessage>>(provider =>
                new RabbitMQEventPublisher<RepositoryFavoritedMessage>(rabbitMQHost, repoFavoritedQueue, provider.GetRequiredService<ILogger<RabbitMQEventPublisher<RepositoryFavoritedMessage>>>(), rabbitMQUser, rabbitMQPassword));
            
            Console.WriteLine($"[ServiceCollectionExtensions.AddInfrastructureLayer] {services};{configuration} OK");
            return services;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ServiceCollectionExtensions.AddInfrastructureLayer] {services};{configuration} {ex.Message}");
            throw;
        }
    }
}