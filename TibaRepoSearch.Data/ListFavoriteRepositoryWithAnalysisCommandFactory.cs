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
        _logger.LogTrace("[ListFavoriteRepositoryWithAnalysisCommandFactory..ctor] {contextFactory} OK", contextFactory);
    }

    public IListFavoriteRepositoryWithAnalysisCommand Create(string userId)
    {
        try
        {
            var result = new ListFavoriteRepositoryWithAnalysisCommand(userId, _contextFactory, _commandLogger);
            _logger.LogTrace("[ListFavoriteRepositoryWithAnalysisCommandFactory.Create] {userId} OK", userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[ListFavoriteRepositoryWithAnalysisCommandFactory.Create] {userId} {Message}", userId, ex.Message);
            throw;
        }
    }
}