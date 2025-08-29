using Microsoft.Extensions.Options;

namespace TibaRepoSearch;

public class RepositorySearchUseCase : IRepositorySearchUseCase
{
    private readonly IGithubClient _githubClient;
    private readonly ICacheThrough _cache;
    private readonly RepositorySearchOptions _options;

    public RepositorySearchUseCase(IGithubClient githubClient, ICacheThrough cache, IOptions<RepositorySearchOptions> options)
    {
        _githubClient = githubClient;
        _cache = cache;
        _options = options.Value;
    }

    public async Task<IEnumerable<Repository>> SearchAsync(string query, int page = 1, int pageSize = 10)
    {
        var cacheKey = $"{_options.CacheKey}:{query}";
        var response = await _cache.Get(cacheKey, async _ => await _githubClient.SearchRepositoriesAsync(query), TimeSpan.FromSeconds(_options.CacheTtlSeconds));
        return response.Items
            .Select(repo => new Repository(
                repo.Name,
                repo.Owner.Login,
                repo.StargazersCount,
                repo.UpdatedAt,
                repo.Description ?? string.Empty,
                repo.Id.ToString()))
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}