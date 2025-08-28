namespace TibaRepoSearch;

public class ListUserFavoritesUseCase : IListUserFavoritesUseCase
{
    public Task<IEnumerable<FavoriteRepository>> ListAsync(string userId)
    {
        return Task.FromResult(Enumerable.Empty<FavoriteRepository>());
    }
}