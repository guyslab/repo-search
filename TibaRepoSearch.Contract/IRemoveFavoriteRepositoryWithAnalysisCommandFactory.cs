namespace TibaRepoSearch;

public interface IRemoveFavoriteRepositoryWithAnalysisCommandFactory
{
    IRemoveFavoriteRepositoryWithAnalysisCommand Create(string repoId, string userId);
}