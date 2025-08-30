using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TibaRepoSearch;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var rabbitMqHost = context.Configuration["RabbitMQ:Host"] ?? "localhost";
        var repoFavoritedQueue = context.Configuration["RabbitMQ:RepoFavoritedQueue"] ?? "repo.favorited";
        var analysisReadyQueue = context.Configuration["RabbitMQ:AnalysisReadyQueue"] ?? "analysis.ready";
        
        services.AddSingleton<IEventPublisher<RepositoryAnalysisReadyMessage>>(provider =>
            new RabbitMQEventPublisher<RepositoryAnalysisReadyMessage>(rabbitMqHost, analysisReadyQueue));
        services.AddSingleton<IRepositoryFavoritedEventHandler, MockRepositoryFavoritedEventHandler>();
        services.AddSingleton<MessageReceiver>(provider => 
            new MessageReceiver(
                provider.GetRequiredService<IRepositoryFavoritedEventHandler>(),
                rabbitMqHost,
                repoFavoritedQueue));
        services.AddHostedService<AnalysisFetcherService>();
    })
    .Build();

await host.RunAsync();
