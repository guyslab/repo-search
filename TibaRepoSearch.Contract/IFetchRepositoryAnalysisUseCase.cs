namespace TibaRepoSearch;

public interface IFetchRepositoryAnalysisUseCase
{
    Task<Analysis?> FetchAnalysisAsync(string repoId);
}