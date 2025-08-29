namespace TibaRepoSearch;

public interface IAddOrUpdateFavoriteRepositoryAnalysisCommandFactory
{
    IAddOrUpdateFavoriteRepositoryAnalysisCommand Create(string repoId, string userId, Analysis analysis);
}