using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class ListFavoriteRepositoryWithAnalysisCommand : IListFavoriteRepositoryWithAnalysisCommand
{
    private readonly IDbContextFactory<FavoriteRepositoriesContext> _contextFactory;
    private readonly string _userId;
    private readonly ILogger<ListFavoriteRepositoryWithAnalysisCommand> _logger;

    public ListFavoriteRepositoryWithAnalysisCommand(string userId, IDbContextFactory<FavoriteRepositoriesContext> contextFactory, ILogger<ListFavoriteRepositoryWithAnalysisCommand> logger)
    {
        _userId = userId;
        _contextFactory = contextFactory;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommand..ctor] {userId};{contextFactory} OK", DateTime.UtcNow.ToString("O"), userId, contextFactory);
    }

    public async Task<IEnumerable<IFavoriteRepositoryData>> ExecuteAsync()
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var result = await context.FavoriteRepositories
                .Include(f => f.Analysis)
                .Where(f => f.UserId == _userId)
                .ToListAsync();
            _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommand.ExecuteAsync]  OK", DateTime.UtcNow.ToString("O"));
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommand.ExecuteAsync]  {Message}", DateTime.UtcNow.ToString("O"), ex.Message);
            throw;
        }
    }
}