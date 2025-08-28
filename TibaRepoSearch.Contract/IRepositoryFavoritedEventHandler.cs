namespace TibaRepoSearch;

public interface IRepositoryFavoritedEventHandler
{
    Task Handle(RepositoryFavoritedMessage message);
}