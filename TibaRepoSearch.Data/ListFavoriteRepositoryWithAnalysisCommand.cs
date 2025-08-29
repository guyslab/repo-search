using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class ListFavoriteRepositoryWithAnalysisCommand : IListFavoriteRepositoryWithAnalysisCommand
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly string _userId;

    public ListFavoriteRepositoryWithAnalysisCommand(string userId, FavoriteRepositoriesContext context)
    {
        _userId = userId;
        _context = context;
    }

    public async Task<IEnumerable<IFavoriteRepositoryData>> ExecuteAsync()
    {
        return await _context.FavoriteRepositories
            .Include(f => f.Analysis)
            .Where(f => f.UserId == _userId)
            .ToListAsync();
    }
}