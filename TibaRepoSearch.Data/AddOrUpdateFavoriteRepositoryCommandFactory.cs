namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryCommandFactory : IAddOrUpdateFavoriteRepositoryCommandFactory
{
    private readonly FavoriteRepositoriesContext _context;

    public AddOrUpdateFavoriteRepositoryCommandFactory(FavoriteRepositoriesContext context)
    {
        _context = context;
    }

    public IAddOrUpdateFavoriteRepositoryCommand Create(string userId, Repository repository)
    {
        var data = new FavoriteRepositoryData
        {
            UserId = userId,
            RepoId = repository.RepoId,
            Name = repository.Name,
            Owner = repository.Owner,
            Stars = repository.Stars,
            UpdatedAt = repository.UpdatedAt
        };

        return new AddOrUpdateFavoriteRepositoryCommand(data, _context);
    }
}