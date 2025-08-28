namespace TibaRepoSearch;

public class GithubClientOptions
{
    public int RetryCount { get; set; } = 3;
    public int DelaySeconds { get; set; } = 1;
}