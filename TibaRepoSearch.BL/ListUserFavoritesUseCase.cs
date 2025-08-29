namespace TibaRepoSearch;

public class ListUserFavoritesUseCase : IListUserFavoritesUseCase
{
    private readonly IListFavoriteRepositoryWithAnalysisCommandFactory _commandFactory;

    public ListUserFavoritesUseCase(IListFavoriteRepositoryWithAnalysisCommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task<IEnumerable<FavoriteRepository>> ListAsync(string userId)
    {
        var command = _commandFactory.Create(userId);
        var favorites = await command.ExecuteAsync();
        return favorites.Select(f => new FavoriteRepository(
            f.Name, 
            f.Owner, 
            f.Stars, 
            f.UpdatedAt, 
            string.Empty, 
            f.RepoId, 
            MapAnalysis(f.Analysis)));
    }
    
    private static Analysis? MapAnalysis(IFavoriteRepositoryAnalysisData? analysis)
    {
        if (analysis == null) return null;
        
        return new Analysis(
            analysis.License ?? string.Empty,
            analysis.TopicsJson?.Split(',') ?? Array.Empty<string>(),
            analysis.PrimaryLanguage ?? string.Empty,
            analysis.ReadmeLength ?? 0,
            analysis.OpenIssues ?? 0,
            analysis.Forks ?? 0,
            analysis.StarsSnapshot ?? 0,
            analysis.ActivityDays ?? 0,
            analysis.DefaultBranch ?? string.Empty,
            analysis.HealthScore ?? 0);
    }
}