using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TibaRepoSearch;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddSimpleConsole(options =>
        {
            options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff] ";
            options.IncludeScopes = false;
        });
        logging.SetMinimumLevel(LogLevel.Trace);
    })
    .ConfigureServices((context, services) =>
    {
        var rabbitMqHost = context.Configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitMqUser = context.Configuration["RabbitMQ:UserName"] ?? "admin";
        var rabbitMqPassword = context.Configuration["RabbitMQ:Password"] ?? "admin";
        var repoFavoritedQueue = context.Configuration["RabbitMQ:RepoFavoritedQueue"] ?? "repo.favorited";
        var analysisReadyQueue = context.Configuration["RabbitMQ:AnalysisReadyQueue"] ?? "analysis.ready";
        
        services.AddSingleton<IEventPublisher<RepositoryAnalysisReadyMessage>>(provider =>
            new RabbitMQEventPublisher<RepositoryAnalysisReadyMessage>(rabbitMqHost, analysisReadyQueue, provider.GetRequiredService<ILogger<RabbitMQEventPublisher<RepositoryAnalysisReadyMessage>>>(), rabbitMqUser, rabbitMqPassword));
        services.Configure<GithubClientOptions>(options => { options.RetryCount = 3; options.DelaySeconds = 1; });
        services.AddHttpClient<IGithubClient, GitHubClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "TibaRepoSearch");
        });
        
        var dbHost = context.Configuration["Database:Host"] ?? "localhost";
        var dbName = context.Configuration["Database:Name"] ?? "tibarepodb";
        var dbUser = context.Configuration["Database:User"] ?? "tibauser";
        var dbPassword = context.Configuration["Database:Password"] ?? "tibapass";
        var connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword}";
        services.AddDbContextFactory<FavoriteRepositoriesContext>(options => options.UseNpgsql(connectionString));
        
        services.AddSingleton<IFetchRepositoryAnalysisUseCase, FetchRepositoryAnalysisUseCase>();
        services.AddSingleton<IAddOrUpdateFavoriteRepositoryAnalysisCommandFactory, AddOrUpdateFavoriteRepositoryAnalysisCommandFactory>();
        services.AddSingleton<IRepositoryFavoritedEventHandler, RepositoryFavoritedEventHandler>();
        services.AddSingleton<MessageReceiver>(provider => 
            new MessageReceiver(
                provider.GetRequiredService<IRepositoryFavoritedEventHandler>(),
                rabbitMqHost,
                repoFavoritedQueue,
                rabbitMqUser,
                rabbitMqPassword));
        services.AddHostedService<AnalysisFetcherService>();
    })
    .Build();

await host.RunAsync();
