using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryCommand : IAddOrUpdateFavoriteRepositoryCommand
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly IFavoriteRepositoryData _data;

    public AddOrUpdateFavoriteRepositoryCommand(IFavoriteRepositoryData data, FavoriteRepositoriesContext context)
    {
        _data = data;
        _context = context;
    }

    public async Task ExecuteAsync()
    {
        var existing = await _context.FavoriteRepositories
            .FirstOrDefaultAsync(f => f.RepoId == _data.RepoId && f.UserId == _data.UserId);

        if (existing != null)
        {
            existing.Name = _data.Name;
            existing.Owner = _data.Owner;
            existing.Stars = _data.Stars;
            existing.UpdatedAt = _data.UpdatedAt;
        }
        else
        {
            _context.FavoriteRepositories.Add(new FavoriteRepositoryData
            {
                Id = Guid.NewGuid(),
                UserId = _data.UserId,
                RepoId = _data.RepoId,
                Name = _data.Name,
                Owner = _data.Owner,
                Stars = _data.Stars,
                UpdatedAt = _data.UpdatedAt,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
    }
}