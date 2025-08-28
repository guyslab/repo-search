namespace TibaRepoSearch;

public class AddToFavoritesUseCase : IAddToFavoritesUseCase
{
    public Task AddAsync(AddFavoriteRequest request, string userId)
    {
        return Task.CompletedTask;
    }
}