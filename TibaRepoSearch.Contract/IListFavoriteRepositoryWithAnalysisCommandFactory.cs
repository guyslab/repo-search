namespace TibaRepoSearch;

public interface IListFavoriteRepositoryWithAnalysisCommandFactory
{
    IListFavoriteRepositoryWithAnalysisCommand Create(string userId);
}