using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class ListFavoriteRepositoryWithAnalysisCommandFactory : IListFavoriteRepositoryWithAnalysisCommandFactory
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly ILogger<ListFavoriteRepositoryWithAnalysisCommandFactory> _logger;
    private readonly ILogger<ListFavoriteRepositoryWithAnalysisCommand> _commandLogger;

    public ListFavoriteRepositoryWithAnalysisCommandFactory(FavoriteRepositoriesContext context, ILogger<ListFavoriteRepositoryWithAnalysisCommandFactory> logger, ILogger<ListFavoriteRepositoryWithAnalysisCommand> commandLogger)
    {
        _context = context;
        _logger = logger;
        _commandLogger = commandLogger;
        _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommandFactory..ctor] {context} OK", DateTime.UtcNow.ToString("O"), context);
    }

    public IListFavoriteRepositoryWithAnalysisCommand Create(string userId)
    {
        try
        {
            var result = new ListFavoriteRepositoryWithAnalysisCommand(userId, _context, _commandLogger);
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