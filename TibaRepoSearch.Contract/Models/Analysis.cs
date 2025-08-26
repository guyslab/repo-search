namespace TibaRepoSearch;

public record Analysis(
    string License,
    string[] Topics,
    string PrimaryLanguage,
    int ReadmeLength,
    int OpenIssues,
    int Forks,
    int StarsSnapshot,
    int ActivityDays,
    string DefaultBranch,
    double HealthScore);