using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class ListFavoriteRepositoryWithAnalysisCommandFactory : IListFavoriteRepositoryWithAnalysisCommandFactory
{
    private readonly IDbContextFactory<FavoriteRepositoriesContext> _contextFactory;
    private readonly ILogger<ListFavoriteRepositoryWithAnalysisCommandFactory> _logger;
    private readonly ILogger<ListFavoriteRepositoryWithAnalysisCommand> _commandLogger;

    public ListFavoriteRepositoryWithAnalysisCommandFactory(IDbContextFactory<FavoriteRepositoriesContext> contextFactory, ILogger<ListFavoriteRepositoryWithAnalysisCommandFactory> logger, ILogger<ListFavoriteRepositoryWithAnalysisCommand> commandLogger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _commandLogger = commandLogger;
        _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommandFactory..ctor] {contextFactory} OK", DateTime.UtcNow.ToString("O"), contextFactory);
    }

    public IListFavoriteRepositoryWithAnalysisCommand Create(string userId)
    {
        try
        {
            var result = new ListFavoriteRepositoryWithAnalysisCommand(userId, _contextFactory, _commandLogger);
            _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommandFactory.Create] {userId} OK", DateTime.UtcNow.ToString("O"), userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommandFactory.Create] {userId} {Message}", DateTime.UtcNow.ToString("O"), userId, ex.Message);
            throw;
        }
    }
}