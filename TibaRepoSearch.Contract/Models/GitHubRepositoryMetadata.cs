using System.Text.Json.Serialization;

namespace TibaRepoSearch;

public record GitHubRepositoryMetadata(
    GitHubLicense? License,
    string[] Topics,
    string? Language,
    int Size,
    [property: JsonPropertyName("open_issues_count")] int OpenIssuesCount,
    [property: JsonPropertyName("forks_count")] int ForksCount,
    [property: JsonPropertyName("stargazers_count")] int StargazersCount,
    [property: JsonPropertyName("pushed_at")] DateTime PushedAt,
    [property: JsonPropertyName("default_branch")] string DefaultBranch
);
