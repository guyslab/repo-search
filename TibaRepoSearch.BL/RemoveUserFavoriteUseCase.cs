namespace TibaRepoSearch;

public class RemoveUserFavoriteUseCase : IRemoveUserFavoriteUseCase
{
    private readonly IRemoveFavoriteRepositoryWithAnalysisCommandFactory _commandFactory;

    public RemoveUserFavoriteUseCase(IRemoveFavoriteRepositoryWithAnalysisCommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task RemoveAsync(string repoId, string userId)
    {
        var command = _commandFactory.Create(repoId, userId);
        await command.ExecuteAsync();
    }
}