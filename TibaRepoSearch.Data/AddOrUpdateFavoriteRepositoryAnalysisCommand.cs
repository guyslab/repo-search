using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryAnalysisCommand : IAddOrUpdateFavoriteRepositoryAnalysisCommand
{
    private readonly FavoriteRepositoriesContext _context;
    private readonly IFavoriteRepositoryAnalysisData _data;
    private readonly string _repoId;
    private readonly string _userId;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryAnalysisCommand> _logger;

    public AddOrUpdateFavoriteRepositoryAnalysisCommand(IFavoriteRepositoryAnalysisData data, string repoId, string userId, FavoriteRepositoriesContext context, ILogger<AddOrUpdateFavoriteRepositoryAnalysisCommand> logger)
    {
        _data = data;
        _repoId = repoId;
        _userId = userId;
        _context = context;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryAnalysisCommand..ctor] {data};{repoId};{userId};{context} OK", DateTime.UtcNow.ToString("O"), data, repoId, userId, context);
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var favorite = await _context.FavoriteRepositories
                .FirstOrDefaultAsync(f => f.RepoId == _repoId && f.UserId == _userId);
            
            if (favorite == null) return;
            
            var existing = await _context.FavoriteRepositoryAnalysis
                .FirstOrDefaultAsync(a => a.FavoriteId == favorite.Id);

            if (existing != null)
            {
                existing.License = _data.License;
                existing.TopicsJson = _data.TopicsJson;
                existing.PrimaryLanguage = _data.PrimaryLanguage;
                existing.ReadmeLength = _data.ReadmeLength;
                existing.OpenIssues = _data.OpenIssues;
                existing.Forks = _data.Forks;
                existing.StarsSnapshot = _data.StarsSnapshot;
                existing.ActivityDays = _data.ActivityDays;
                existing.DefaultBranch = _data.DefaultBranch;
                existing.HealthScore = _data.HealthScore;
            }
            else
            {
                _context.FavoriteRepositoryAnalysis.Add(new FavoriteRepositoryAnalysisData
                {
                    Id = Guid.NewGuid(),
                    FavoriteId = favorite.Id,
                    License = _data.License,
                    TopicsJson = _data.TopicsJson,
                    PrimaryLanguage = _data.PrimaryLanguage,
                    ReadmeLength = _data.ReadmeLength,
                    OpenIssues = _data.OpenIssues,
                    Forks = _data.Forks,
                    StarsSnapshot = _data.StarsSnapshot,
                    ActivityDays = _data.ActivityDays,
                    DefaultBranch = _data.DefaultBranch,
                    HealthScore = _data.HealthScore,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryAnalysisCommand.ExecuteAsync]  OK", DateTime.UtcNow.ToString("O"));
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryAnalysisCommand.ExecuteAsync]  {Message}", DateTime.UtcNow.ToString("O"), ex.Message);
            throw;
        }
    }
}