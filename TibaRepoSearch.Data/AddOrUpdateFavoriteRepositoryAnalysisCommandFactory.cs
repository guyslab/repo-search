using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class AddOrUpdateFavoriteRepositoryAnalysisCommandFactory : IAddOrUpdateFavoriteRepositoryAnalysisCommandFactory
{
    private readonly IDbContextFactory<FavoriteRepositoriesContext> _contextFactory;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryAnalysisCommandFactory> _logger;
    private readonly ILogger<AddOrUpdateFavoriteRepositoryAnalysisCommand> _commandLogger;

    public AddOrUpdateFavoriteRepositoryAnalysisCommandFactory(IDbContextFactory<FavoriteRepositoriesContext> contextFactory, ILogger<AddOrUpdateFavoriteRepositoryAnalysisCommandFactory> logger, ILogger<AddOrUpdateFavoriteRepositoryAnalysisCommand> commandLogger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _commandLogger = commandLogger;
        _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryAnalysisCommandFactory..ctor] {contextFactory} OK", DateTime.UtcNow.ToString("O"), contextFactory);
    }

    public IAddOrUpdateFavoriteRepositoryAnalysisCommand Create(string repoId, string userId, Analysis analysis)
    {
        try
        {
            var data = new FavoriteRepositoryAnalysisData
            {
                License = analysis.License,
                TopicsJson = string.Join(",", analysis.Topics),
                PrimaryLanguage = analysis.PrimaryLanguage,
                ReadmeLength = analysis.ReadmeLength,
                OpenIssues = analysis.OpenIssues,
                Forks = analysis.Forks,
                StarsSnapshot = analysis.StarsSnapshot,
                ActivityDays = analysis.ActivityDays,
                DefaultBranch = analysis.DefaultBranch,
                HealthScore = analysis.HealthScore
            };

            var result = new AddOrUpdateFavoriteRepositoryAnalysisCommand(data, repoId, userId, _contextFactory, _commandLogger);
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryAnalysisCommandFactory.Create] {repoId};{userId};{analysis} OK", DateTime.UtcNow.ToString("O"), repoId, userId, analysis);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [AddOrUpdateFavoriteRepositoryAnalysisCommandFactory.Create] {repoId};{userId};{analysis} {Message}", DateTime.UtcNow.ToString("O"), repoId, userId, analysis, ex.Message);
            throw;
        }
    }
}