using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class FetchRepositoryAnalysisUseCase : IFetchRepositoryAnalysisUseCase
{
    private readonly IGithubClient _githubClient;
    private readonly ILogger<FetchRepositoryAnalysisUseCase> _logger;

    public FetchRepositoryAnalysisUseCase(IGithubClient githubClient, ILogger<FetchRepositoryAnalysisUseCase> logger)
    {
        _githubClient = githubClient;
        _logger = logger;
    }
    public async Task<Analysis?> FetchAnalysisAsync(string repoId)
    {
        try
        {
            var metadata = await _githubClient.FetchRepositoryMetadataAsync(repoId);
            if (metadata == null)
            {
                _logger.LogTrace("[FetchRepositoryAnalysisUseCase.FetchAnalysisAsync] {repoId} No metadata found", repoId);
                return null;
            }

            var activityDays = (DateTime.UtcNow - metadata.PushedAt).Days;
            var healthScore = CalculateHealthScore(metadata.StargazersCount, activityDays, metadata.OpenIssuesCount, metadata.ForksCount);

            var analysis = new Analysis(
                metadata.License?.SpdxId ?? "Unknown",
                metadata.Topics,
                metadata.Language ?? "Unknown",
                metadata.Size,
                metadata.OpenIssuesCount,
                metadata.ForksCount,
                metadata.StargazersCount,
                activityDays,
                metadata.DefaultBranch,
                healthScore
            );

            _logger.LogTrace("[FetchRepositoryAnalysisUseCase.FetchAnalysisAsync] {repoId} OK", repoId);
            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[FetchRepositoryAnalysisUseCase.FetchAnalysisAsync] {repoId} {Message}", repoId, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Calculates a repository health score based on three weighted factors:
    /// - Stars Weight (50%): Popularity based on star count, normalized to 1000 stars = max 50 points
    /// - Activity Weight (30%): Recent activity based on days since last push, within 365 days = max 30 points
    /// - Issue/Fork Ratio Weight (20%): Quality indicator based on open issues relative to forks, lower ratio = max 20 points
    /// </summary>
    /// <param name="stars">Number of repository stars</param>
    /// <param name="activityDays">Days since last push</param>
    /// <param name="openIssues">Number of open issues</param>
    /// <param name="forks">Number of repository forks</param>
    /// <returns>Health score from 0 to 100</returns>
    public decimal CalculateHealthScore(int stars, int activityDays, int openIssues, int forks)
    {
        var starsWeight = Math.Min(stars / 1000.0m, 1.0m) * 50;
        var activityWeight = Math.Max(0, 1 - (activityDays / 365.0m)) * 30;
        var issueRatio = forks > 0 ? (decimal)openIssues / forks : openIssues > 0 ? 1 : 0;
        var ratioWeight = Math.Max(0, 1 - issueRatio) * 20;
        return Math.Min(100, starsWeight + activityWeight + ratioWeight);
    }

}