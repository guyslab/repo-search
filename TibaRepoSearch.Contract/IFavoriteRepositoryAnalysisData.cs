namespace TibaRepoSearch;

public interface IFavoriteRepositoryAnalysisData
{
    Guid Id { get; set; }
    Guid FavoriteId { get; set; }
    string? License { get; set; }
    string? TopicsJson { get; set; }
    string? PrimaryLanguage { get; set; }
    int? ReadmeLength { get; set; }
    int? OpenIssues { get; set; }
    int? Forks { get; set; }
    int? StarsSnapshot { get; set; }
    int? ActivityDays { get; set; }
    string? DefaultBranch { get; set; }
    decimal? HealthScore { get; set; }
    DateTime CreatedAt { get; set; }
}