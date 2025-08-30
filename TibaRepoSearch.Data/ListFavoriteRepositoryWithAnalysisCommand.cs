using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class ListFavoriteRepositoryWithAnalysisCommand : IListFavoriteRepositoryWithAnalysisCommand
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly string _userId;
    private readonly ILogger<ListFavoriteRepositoryWithAnalysisCommand> _logger;

    public ListFavoriteRepositoryWithAnalysisCommand(string userId, FavoriteRepositoriesContext context, ILogger<ListFavoriteRepositoryWithAnalysisCommand> logger)
    {
        _userId = userId;
        _context = context;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [ListFavoriteRepositoryWithAnalysisCommand..ctor] {userId};{context} OK", DateTime.UtcNow.ToString("O"), userId, context);
    }

    public async Task<IEnumerable<IFavoriteRepositoryData>> ExecuteAsync()
    {
        try
        {
            var result = await _context.FavoriteRepositories
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