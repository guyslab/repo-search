namespace TibaRepoSearch;

public class RepositoryFavoritedEventHandler : IRepositoryFavoritedEventHandler
{
    private readonly IEventPublisher<RepositoryAnalysisReadyMessage> _eventPublisher;
    private readonly IFetchRepositoryAnalysisUseCase _fetchAnalysisUseCase;
    private readonly IAddOrUpdateFavoriteRepositoryAnalysisCommandFactory _commandFactory;

    public RepositoryFavoritedEventHandler(IEventPublisher<RepositoryAnalysisReadyMessage> eventPublisher, IFetchRepositoryAnalysisUseCase fetchAnalysisUseCase, IAddOrUpdateFavoriteRepositoryAnalysisCommandFactory commandFactory)
    {
        _eventPublisher = eventPublisher;
        _fetchAnalysisUseCase = fetchAnalysisUseCase;
        _commandFactory = commandFactory;
    }

    public async Task Handle(RepositoryFavoritedMessage message)
    {
        Console.WriteLine($"Received repository favorited message for RepoId: {message.RepoId}");
        
        var analysis = await _fetchAnalysisUseCase.FetchAnalysisAsync(message.RepoId);
        if (analysis == null)
        {
            Console.WriteLine($"No analysis found for RepoId: {message.RepoId}");
            return;
        }
        
        var command = _commandFactory.Create(message.RepoId, message.UserId, analysis);
        await command.ExecuteAsync();
        
        var analysisReady = new RepositoryAnalysisReadyMessage(
                message.RepoId,
                analysis.License,
                analysis.Topics,
                analysis.PrimaryLanguage,
                analysis.ReadmeLength,
                analysis.OpenIssues,
                analysis.Forks,
                analysis.StarsSnapshot,
                analysis.ActivityDays,
                analysis.DefaultBranch,
                analysis.HealthScore
            );

        await _eventPublisher.PublishAsync(analysisReady);
        Console.WriteLine($"Published analysis ready message for RepoId: {message.RepoId}");
    }
}