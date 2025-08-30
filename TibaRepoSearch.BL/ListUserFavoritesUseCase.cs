using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class ListUserFavoritesUseCase : IListUserFavoritesUseCase
{
    private readonly IListFavoriteRepositoryWithAnalysisCommandFactory _commandFactory;
    private readonly ILogger<ListUserFavoritesUseCase> _logger;

    public ListUserFavoritesUseCase(IListFavoriteRepositoryWithAnalysisCommandFactory commandFactory, ILogger<ListUserFavoritesUseCase> logger)
    {
        _commandFactory = commandFactory;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [ListUserFavoritesUseCase..ctor] {commandFactory} OK", DateTime.UtcNow.ToString("O"), commandFactory);
    }

    public async Task<IEnumerable<FavoriteRepository>> ListAsync(string userId)
    {
        try
        {
            var command = _commandFactory.Create(userId);
            var favorites = await command.ExecuteAsync();
            var result = favorites.Select(f => new FavoriteRepository(
                f.Name, 
                f.Owner, 
                f.Stars, 
                f.UpdatedAt, 
                string.Empty, 
                f.RepoId, 
                MapAnalysis(f.Analysis)));
            _logger.LogTrace("[{timestamp}] [ListUserFavoritesUseCase.ListAsync] {userId} OK", DateTime.UtcNow.ToString("O"), userId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [ListUserFavoritesUseCase.ListAsync] {userId} {Message}", DateTime.UtcNow.ToString("O"), userId, ex.Message);
            throw;
        }
    }
    
    private Analysis? MapAnalysis(IFavoriteRepositoryAnalysisData? analysis)
    {
        try
        {
            if (analysis == null) return null;
            
            var result = new Analysis(
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
            _logger.LogTrace("[{timestamp}] [ListUserFavoritesUseCase.MapAnalysis] {analysis} OK", DateTime.UtcNow.ToString("O"), analysis);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [ListUserFavoritesUseCase.MapAnalysis] {analysis} {Message}", DateTime.UtcNow.ToString("O"), analysis, ex.Message);
            throw;
        }
    }
}