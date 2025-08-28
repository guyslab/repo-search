namespace TibaRepoSearch;

public interface ICacheThrough
{
    /// <summary>
    /// Gets the value for the specified key, or calls the onMiss function to get and cache the value if the key is not found in the cache.
    /// </summary>
    Task<TResult> Get<TResult>(string key, Func<string, Task<TResult>> onMiss, TimeSpan ttl);
}