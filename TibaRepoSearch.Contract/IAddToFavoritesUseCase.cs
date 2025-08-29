namespace TibaRepoSearch;

public interface IAddToFavoritesUseCase
{
    Task AddAsync(AddFavoriteRequest request, string userId);
}