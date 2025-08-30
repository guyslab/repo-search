namespace TibaRepoSearch;

public class MockRepositoryFavoritedEventHandler : IRepositoryFavoritedEventHandler
{
    private readonly IEventPublisher<RepositoryAnalysisReadyMessage> _eventPublisher;

    public MockRepositoryFavoritedEventHandler(IEventPublisher<RepositoryAnalysisReadyMessage> eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(RepositoryFavoritedMessage message)
    {
        Console.WriteLine($"Received repository favorited message for RepoId: {message.RepoId}");
        
        // Mock analysis data
        var analysisReady = new RepositoryAnalysisReadyMessage(
            message.RepoId,
            "MIT",
            new[] { "web", "api" },
            "C#",
            1500,
            5,
            10,
            100,
            30,
            "main",
            85
        );
        
        await _eventPublisher.PublishAsync(analysisReady);
        Console.WriteLine($"Published analysis ready message for RepoId: {message.RepoId}");
    }
}