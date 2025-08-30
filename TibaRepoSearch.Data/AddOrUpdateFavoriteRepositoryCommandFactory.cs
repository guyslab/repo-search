using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryCommandFactory : IAddOrUpdateFavoriteRepositoryCommandFactory
{
    private readonly IDbContextFactory<FavoriteRepositoriesContext> _contextFactory;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryCommandFactory> _logger;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryCommand> _commandLogger;

    public AddOrUpdateFavoriteRepositoryCommandFactory(IDbContextFactory<FavoriteRepositoriesContext> contextFactory, ILogger<AddOrUpdateFavoriteRepositoryCommandFactory> logger, ILogger<AddOrUpdateFavoriteRepositoryCommand> commandLogger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _commandLogger = commandLogger;
        _logger.LogTrace("[AddOrUpdateFavoriteRepositoryCommandFactory..ctor] {contextFactory} OK", contextFactory);
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

            var result = new AddOrUpdateFavoriteRepositoryCommand(data, _contextFactory, _commandLogger);
            _logger.LogTrace("[AddOrUpdateFavoriteRepositoryCommandFactory.Create] {userId};{repository} OK", userId, repository);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[AddOrUpdateFavoriteRepositoryCommandFactory.Create] {userId};{repository} {Message}", userId, repository, ex.Message);
            throw;
        }
    }
}