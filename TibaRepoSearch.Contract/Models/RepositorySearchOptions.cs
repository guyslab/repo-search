namespace TibaRepoSearch;

public class RepositorySearchOptions
{
    public int CacheTtlSeconds { get; set; } = 60;
    public string CacheKey { get; set; } = "search";
}