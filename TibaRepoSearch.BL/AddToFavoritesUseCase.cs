namespace TibaRepoSearch;

public class AddToFavoritesUseCase : IAddToFavoritesUseCase
{
    private readonly IAddOrUpdateFavoriteRepositoryCommandFactory _commandFactory;

    public AddToFavoritesUseCase(IAddOrUpdateFavoriteRepositoryCommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task AddAsync(AddFavoriteRequest request, string userId)
    {
        var repository = new Repository(request.Name, request.Owner, request.Stars, request.UpdatedAt, string.Empty, request.RepoId);
        var command = _commandFactory.Create(userId, repository);
        await command.ExecuteAsync();
    }
}