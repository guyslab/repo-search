using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class RemoveFavoriteRepositoryWithAnalysisCommand : IRemoveFavoriteRepositoryWithAnalysisCommand
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly string _repoId;
    private readonly string _userId;

    public RemoveFavoriteRepositoryWithAnalysisCommand(string repoId, string userId, FavoriteRepositoriesContext context)
    {
        _repoId = repoId;
        _userId = userId;
        _context = context;
    }

    public async Task ExecuteAsync()
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
    }
}