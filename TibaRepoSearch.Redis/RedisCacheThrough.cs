using StackExchange.Redis;
using System.Text.Json;

namespace TibaRepoSearch;

public class RedisCacheThrough : ICacheThrough
{
    private readonly IDatabase _database;

    public RedisCacheThrough(IDatabase database)
    {
        _database = database;
    }

    public async Task<TResult> Get<TResult>(string key, Func<string, Task<TResult>> onMiss, TimeSpan ttl)
    {
        var cachedValue = await _database.StringGetAsync(key);
        
        if (cachedValue.HasValue)
        {
            return JsonSerializer.Deserialize<TResult>(cachedValue!)!;
        }

        var result = await onMiss(key);
        var serializedResult = JsonSerializer.Serialize(result);
        await _database.StringSetAsync(key, serializedResult, ttl);
        
        return result;
    }
}