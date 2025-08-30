namespace TibaRepoSearch;

public class MockRepositoryFavoritedEventHandler : IRepositoryFavoritedEventHandler
{
    public Task Handle(RepositoryFavoritedMessage message)
    {
        Console.WriteLine($"Received repository favorited message for RepoId: {message.RepoId}");
        return Task.CompletedTask;
    }
}