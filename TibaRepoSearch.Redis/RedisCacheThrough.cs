using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RedisCacheThrough : ICacheThrough
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheThrough> _logger;

    public RedisCacheThrough(IDatabase database, ILogger<RedisCacheThrough> logger)
    {
        _database = database;
        _logger = logger;
        _logger.LogTrace("[RedisCacheThrough..ctor] {database} OK", database);
    }

    public async Task<TResult> Get<TResult>(string key, Func<string, Task<TResult>> onMiss, TimeSpan ttl)
    {
        try
        {
            var cachedValue = await _database.StringGetAsync(key);
            
            if (cachedValue.HasValue)
            {
                var result = JsonSerializer.Deserialize<TResult>(cachedValue!)!;
                _logger.LogTrace("[RedisCacheThrough.Get] {key};{onMiss};{ttl} OK", key, onMiss, ttl);
                return result;
            }

            var missResult = await onMiss(key);
            var serializedResult = JsonSerializer.Serialize(missResult);
            await _database.StringSetAsync(key, serializedResult, ttl);
            
            _logger.LogTrace("[RedisCacheThrough.Get] {key};{onMiss};{ttl} OK", key, onMiss, ttl);
            return missResult;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[RedisCacheThrough.Get] {key};{onMiss};{ttl} {Message}", key, onMiss, ttl, ex.Message);
            throw;
        }
    }
}