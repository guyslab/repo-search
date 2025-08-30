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
        _logger.LogTrace("[RemoveFavoriteRepositoryWithAnalysisCommandFactory..ctor] {contextFactory} OK", contextFactory);
    }

    public IRemoveFavoriteRepositoryWithAnalysisCommand Create(string repoId, string userId)
    {
        try
        {
            var result = new RemoveFavoriteRepositoryWithAnalysisCommand(repoId, userId, _contextFactory, _commandLogger);
            _logger.LogTrace("[RemoveFavoriteRepositoryWithAnalysisCommandFactory.Create] {repoId};{userId} OK", repoId, userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[RemoveFavoriteRepositoryWithAnalysisCommandFactory.Create] {repoId};{userId} {Message}", repoId, userId, ex.Message);
            throw;
        }
    }
}