using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RepositorySearchUseCase : IRepositorySearchUseCase
{
    private readonly IGithubClient _githubClient;
    private readonly ICacheThrough _cache;
    private readonly RepositorySearchOptions _options;
    private readonly ILogger<RepositorySearchUseCase> _logger;

    public RepositorySearchUseCase(IGithubClient githubClient, ICacheThrough cache, IOptions<RepositorySearchOptions> options, ILogger<RepositorySearchUseCase> logger)
    {
        _githubClient = githubClient;
        _cache = cache;
        _options = options.Value;
        _logger = logger;
        _logger.LogTrace("[RepositorySearchUseCase..ctor] {githubClient};{cache};{options} OK", githubClient, cache, options);
    }

    public async Task<IEnumerable<Repository>> SearchAsync(string query, int page = 1, int pageSize = 10)
    {
        try
        {
            var cacheKey = $"{_options.CacheKey}:{query}";
            var response = await _cache.Get(cacheKey, async _ => await _githubClient.SearchRepositoriesAsync(query), TimeSpan.FromSeconds(_options.CacheTtlSeconds));
            var result = response.Items
                .Select(repo => new Repository(
                    repo.Name,
                    repo.Owner.Login,
                    repo.StargazersCount,
                    repo.UpdatedAt,
                    repo.Description ?? string.Empty,
                    repo.Id.ToString()))
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            _logger.LogTrace("[RepositorySearchUseCase.SearchAsync] {query};{page};{pageSize} OK", query, page, pageSize);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[RepositorySearchUseCase.SearchAsync] {query};{page};{pageSize} {Message}", query, page, pageSize, ex.Message);
            throw;
        }
    }
}