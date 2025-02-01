using System.Text.Json;
using StackExchange.Redis;

namespace Data;

public class RedisService
{
    private readonly IDatabase _database;

    public RedisService(string redisConnectionString = "localhost:6379")
    {
        var redis = ConnectionMultiplexer.Connect(redisConnectionString);
        _database = redis.GetDatabase();
    }
    
    public async Task SaveObjectAsync<T>(string key, T obj)
    {
        string json = JsonSerializer.Serialize(obj);
        await _database.StringSetAsync(key, json);
    }
    public async Task<T?> GetObjectAsync<T>(string key)
    {
        string? json = await _database.StringGetAsync(key);
        return json is not null ? JsonSerializer.Deserialize<T>(json) : default;
    }
}