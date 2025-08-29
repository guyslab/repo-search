namespace TibaRepoSearch;

public class RemoveFavoriteRepositoryWithAnalysisCommandFactory : IRemoveFavoriteRepositoryWithAnalysisCommandFactory
{
    private readonly FavoriteRepositoriesContext _context;

    public RemoveFavoriteRepositoryWithAnalysisCommandFactory(FavoriteRepositoriesContext context)
    {
        _context = context;
    }

    public IRemoveFavoriteRepositoryWithAnalysisCommand Create(string repoId, string userId)
    {
        return new RemoveFavoriteRepositoryWithAnalysisCommand(repoId, userId, _context);
    }
}