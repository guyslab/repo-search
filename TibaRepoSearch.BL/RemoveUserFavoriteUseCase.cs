using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RemoveUserFavoriteUseCase : IRemoveUserFavoriteUseCase
{
    private readonly IRemoveFavoriteRepositoryWithAnalysisCommandFactory _commandFactory;
    private readonly ILogger<RemoveUserFavoriteUseCase> _logger;

    public RemoveUserFavoriteUseCase(IRemoveFavoriteRepositoryWithAnalysisCommandFactory commandFactory, ILogger<RemoveUserFavoriteUseCase> logger)
    {
        _commandFactory = commandFactory;
        _logger = logger;
        _logger.LogTrace("[RemoveUserFavoriteUseCase..ctor] {commandFactory} OK", commandFactory);
    }

    public async Task RemoveAsync(string repoId, string userId)
    {
        try
        {
            var command = _commandFactory.Create(repoId, userId);
            await command.ExecuteAsync();
            _logger.LogTrace("[RemoveUserFavoriteUseCase.RemoveAsync] {repoId};{userId} OK", repoId, userId);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[RemoveUserFavoriteUseCase.RemoveAsync] {repoId};{userId} {Message}", repoId, userId, ex.Message);
            throw;
        }
    }
}