namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryAnalysisCommandFactory : IAddOrUpdateFavoriteRepositoryAnalysisCommandFactory
{
    private readonly FavoriteRepositoriesContext _context;

    public AddOrUpdateFavoriteRepositoryAnalysisCommandFactory(FavoriteRepositoriesContext context)
    {
        _context = context;
    }

    public IAddOrUpdateFavoriteRepositoryAnalysisCommand Create(string repoId, string userId, Analysis analysis)
    {
        var data = new FavoriteRepositoryAnalysisData
        {
            License = analysis.License,
            TopicsJson = "[" + string.Join(",", analysis.Topics) + "]",
            PrimaryLanguage = analysis.PrimaryLanguage,
            ReadmeLength = analysis.ReadmeLength,
            OpenIssues = analysis.OpenIssues,
            Forks = analysis.Forks,
            StarsSnapshot = analysis.StarsSnapshot,
            ActivityDays = analysis.ActivityDays,
            DefaultBranch = analysis.DefaultBranch,
            HealthScore = analysis.HealthScore
        };

        return new AddOrUpdateFavoriteRepositoryAnalysisCommand(data, repoId, userId, _context);
    }
}