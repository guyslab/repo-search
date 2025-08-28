namespace TibaRepoSearch;

public record RepositoryAnalysisReadyMessage(
    string RepoId,
    string License,
    string[] Topics,
    string PrimaryLanguage,
    int ReadmeLength,
    int OpenIssues,
    int Forks,
    int StarsSnapshot,
    int ActivityDays,
    string DefaultBranch,
    int HealthScore
);