using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RemoveFavoriteRepositoryWithAnalysisCommand : IRemoveFavoriteRepositoryWithAnalysisCommand
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly string _repoId;
    private readonly string _userId;
    private readonly ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> _logger;

    public RemoveFavoriteRepositoryWithAnalysisCommand(string repoId, string userId, FavoriteRepositoriesContext context, ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> logger)
    {
        _repoId = repoId;
        _userId = userId;
        _context = context;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommand..ctor] {repoId};{userId};{context} OK", DateTime.UtcNow.ToString("O"), repoId, userId, context);
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var favorite = await _context.FavoriteRepositories
                .FirstOrDefaultAsync(f => f.RepoId == _repoId && f.UserId == _userId);

            if (favorite != null)
            {
                var analysis = await _context.FavoriteRepositoryAnalysis
                    .Where(a => a.FavoriteId == favorite.Id)
                    .ToListAsync();

                _context.FavoriteRepositoryAnalysis.RemoveRange(analysis);
                _context.FavoriteRepositories.Remove(favorite);

                await _context.SaveChangesAsync();
            }
            _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommand.ExecuteAsync]  OK", DateTime.UtcNow.ToString("O"));
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommand.ExecuteAsync]  {Message}", DateTime.UtcNow.ToString("O"), ex.Message);
            throw;
        }
    }
}