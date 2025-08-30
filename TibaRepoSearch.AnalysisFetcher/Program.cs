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
