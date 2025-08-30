using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RemoveFavoriteRepositoryWithAnalysisCommand : IRemoveFavoriteRepositoryWithAnalysisCommand
{
    private readonly IDbContextFactory<FavoriteRepositoriesContext> _contextFactory;
    private readonly string _repoId;
    private readonly string _userId;
    private readonly ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> _logger;

    public RemoveFavoriteRepositoryWithAnalysisCommand(string repoId, string userId, IDbContextFactory<FavoriteRepositoriesContext> contextFactory, ILogger<RemoveFavoriteRepositoryWithAnalysisCommand> logger)
    {
        _repoId = repoId;
        _userId = userId;
        _contextFactory = contextFactory;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [RemoveFavoriteRepositoryWithAnalysisCommand..ctor] {repoId};{userId};{contextFactory} OK", DateTime.UtcNow.ToString("O"), repoId, userId, contextFactory);
    }

    public async Task ExecuteAsync()
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var favorite = await context.FavoriteRepositories
                .FirstOrDefaultAsync(f => f.RepoId == _repoId && f.UserId == _userId);

            if (favorite != null)
            {
                var analysis = await context.FavoriteRepositoryAnalysis
                    .Where(a => a.FavoriteId == favorite.Id)
                    .ToListAsync();

                context.FavoriteRepositoryAnalysis.RemoveRange(analysis);
                context.FavoriteRepositories.Remove(favorite);

                await context.SaveChangesAsync();
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