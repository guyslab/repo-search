using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryCommandFactory : IAddOrUpdateFavoriteRepositoryCommandFactory
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryCommandFactory> _logger;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryCommand> _commandLogger;

    public AddOrUpdateFavoriteRepositoryCommandFactory(FavoriteRepositoriesContext context, ILogger<AddOrUpdateFavoriteRepositoryCommandFactory> logger, ILogger<AddOrUpdateFavoriteRepositoryCommand> commandLogger)
    {
        _context = context;
        _logger = logger;
        _commandLogger = commandLogger;
        _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryCommandFactory..ctor] {context} OK", DateTime.UtcNow.ToString("O"), context);
    }

    public IAddOrUpdateFavoriteRepositoryCommand Create(string userId, Repository repository)
    {
        try
        {
            var data = new FavoriteRepositoryData
            {
                UserId = userId,
                RepoId = repository.RepoId,
                Name = repository.Name,
                Owner = repository.Owner,
                Stars = repository.Stars,
                UpdatedAt = repository.UpdatedAt
            };

            var result = new AddOrUpdateFavoriteRepositoryCommand(data, _context, _commandLogger);
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryCommandFactory.Create] {userId};{repository} OK", DateTime.UtcNow.ToString("O"), userId, repository);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryCommandFactory.Create] {userId};{repository} {Message}", DateTime.UtcNow.ToString("O"), userId, repository, ex.Message);
            throw;
        }
    }
}