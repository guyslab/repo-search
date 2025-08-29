namespace TibaRepoSearch;

public interface IAddOrUpdateFavoriteRepositoryCommandFactory
{
    IAddOrUpdateFavoriteRepositoryCommand Create(string userId, Repository repository);
}