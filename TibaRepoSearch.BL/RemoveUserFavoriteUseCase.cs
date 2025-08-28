namespace TibaRepoSearch;

public class RemoveUserFavoriteUseCase : IRemoveUserFavoriteUseCase
{
    public Task RemoveAsync(string repoId, string userId)
    {
        return Task.CompletedTask;
    }
}