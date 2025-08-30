using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class RemoveFavoriteRepositoryWithAnalysisCommandFactory : IRemoveFavoriteRepositoryWithAnalysisCommandFactory
{
    private readonly IDbContextFactory<FavoriteRepositoriesContext> _contextFactory;
    private readonly ILogger<RemoveFavoriteRepositoryWithAnalysisCommandFactory> _logger;
    private readonly ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> _commandLogger;

    public RemoveFavoriteRepositoryWithAnalysisCommandFactory(IDbContextFactory<FavoriteRepositoriesContext> contextFactory, ILogger<RemoveFavoriteRepositoryWithAnalysisCommandFactory> logger, ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> commandLogger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _commandLogger = commandLogger;
        _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommandFactory..ctor] {contextFactory} OK", DateTime.UtcNow.ToString("O"), contextFactory);
    }

    public IRemoveFavoriteRepositoryWithAnalysisCommand Create(string repoId, string userId)
    {
        try
        {
            var result = new RemoveFavoriteRepositoryWithAnalysisCommand(repoId, userId, _contextFactory, _commandLogger);
            _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommandFactory.Create] {repoId};{userId} OK", DateTime.UtcNow.ToString("O"), repoId, userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommandFactory.Create] {repoId};{userId} {Message}", DateTime.UtcNow.ToString("O"), repoId, userId, ex.Message);
            throw;
        }
    }
}