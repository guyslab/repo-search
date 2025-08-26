namespace TibaRepoSearch;

public interface IAddToFavoritesUseCase
{
    Task AddAsync(AddFavoriteRequest request, string userId);
}

public interface IFetchRepositoryAnalysisUseCase
{
    Task<Analysis?> FetchAnalysisAsync(string repoId);
}

public interface IListUserFavoritesUseCase
{
    Task<IEnumerable<FavoriteRepository>> ListAsync(string userId);
}

public interface IRemoveUserFavoriteUseCase
{
    Task RemoveAsync(string repoId, string userId);
}