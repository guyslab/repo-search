using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class FetchRepositoryAnalysisUseCase : IFetchRepositoryAnalysisUseCase
{
    private readonly ILogger<FetchRepositoryAnalysisUseCase> _logger;

    public FetchRepositoryAnalysisUseCase(ILogger<FetchRepositoryAnalysisUseCase> logger)
    {
        _logger = logger;
    }
    public Task<Analysis?> FetchAnalysisAsync(string repoId)
    {
        try
        {
            var result = Task.FromResult<Analysis?>(null);
            _logger.LogTrace("[{timestamp}] [FetchRepositoryAnalysisUseCase.FetchAnalysisAsync] {repoId} OK", DateTime.UtcNow.ToString("O"), repoId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [FetchRepositoryAnalysisUseCase.FetchAnalysisAsync] {repoId} {Message}", DateTime.UtcNow.ToString("O"), repoId, ex.Message);
            throw;
        }
    }
}