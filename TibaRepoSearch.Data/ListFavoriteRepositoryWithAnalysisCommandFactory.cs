namespace TibaRepoSearch;

public class ListFavoriteRepositoryWithAnalysisCommandFactory : IListFavoriteRepositoryWithAnalysisCommandFactory
{
    private readonly FavoriteRepositoriesContext _context;

    public ListFavoriteRepositoryWithAnalysisCommandFactory(FavoriteRepositoriesContext context)
    {
        _context = context;
    }

    public IListFavoriteRepositoryWithAnalysisCommand Create(string userId)
    {
        return new ListFavoriteRepositoryWithAnalysisCommand(userId, _context);
    }
}