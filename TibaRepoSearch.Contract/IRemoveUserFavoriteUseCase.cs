namespace TibaRepoSearch;

public interface IRemoveUserFavoriteUseCase
{
    Task RemoveAsync(string repoId, string userId);
}