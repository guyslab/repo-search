using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryCommand : IAddOrUpdateFavoriteRepositoryCommand
{
    private readonly IDbContextFactory<FavoriteRepositoriesContext> _contextFactory;
    private readonly IFavoriteRepositoryData _data;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryCommand> _logger;

    public AddOrUpdateFavoriteRepositoryCommand(IFavoriteRepositoryData data, IDbContextFactory<FavoriteRepositoriesContext> contextFactory, ILogger<AddOrUpdateFavoriteRepositoryCommand> logger)
    {
        _data = data;
        _contextFactory = contextFactory;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryCommand..ctor] {data};{contextFactory} OK", DateTime.UtcNow.ToString("O"), data, contextFactory);
    }

    public async Task ExecuteAsync()
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            var existing = await context.FavoriteRepositories
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
                context.FavoriteRepositories.Add(new FavoriteRepositoryData
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

            await context.SaveChangesAsync();
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryCommand.ExecuteAsync]  OK", DateTime.UtcNow.ToString("O"));
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryCommand.ExecuteAsync]  {Message}", DateTime.UtcNow.ToString("O"), ex.Message);
            throw;
        }
    }
}