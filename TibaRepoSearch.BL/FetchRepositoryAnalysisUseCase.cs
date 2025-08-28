namespace TibaRepoSearch;

public class FetchRepositoryAnalysisUseCase : IFetchRepositoryAnalysisUseCase
{
    public Task<Analysis?> FetchAnalysisAsync(string repoId)
    {
        return Task.FromResult<Analysis?>(null);
    }
}