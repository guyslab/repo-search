namespace TibaRepoSearch;

public interface IListUserFavoritesUseCase
{
    Task<IEnumerable<FavoriteRepository>> ListAsync(string userId);
}