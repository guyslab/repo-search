using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RemoveFavoriteRepositoryWithAnalysisCommandFactory : IRemoveFavoriteRepositoryWithAnalysisCommandFactory
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly ILogger<RemoveFavoriteRepositoryWithAnalysisCommandFactory> _logger;
    private readonly ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> _commandLogger;

    public RemoveFavoriteRepositoryWithAnalysisCommandFactory(FavoriteRepositoriesContext context, ILogger<RemoveFavoriteRepositoryWithAnalysisCommandFactory> logger, ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> commandLogger)
    {
        _context = context;
        _logger = logger;
        _commandLogger = commandLogger;
        _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommandFactory..ctor] {context} OK", DateTime.UtcNow.ToString("O"), context);
    }

    public IRemoveFavoriteRepositoryWithAnalysisCommand Create(string repoId, string userId)
    {
        try
        {
            var result = new RemoveFavoriteRepositoryWithAnalysisCommand(repoId, userId, _context, _commandLogger);
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