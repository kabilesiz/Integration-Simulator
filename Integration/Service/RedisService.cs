using Integration.Interfaces;
using StackExchange.Redis;

namespace Integration.Service;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;
    private const string RedisSetKey = "AlreadySavedItems";
    
    public RedisService(string connectionString)
    {
        var redis = ConnectionMultiplexer.Connect($"allowAdmin=true, {connectionString}");
        _database = redis.GetDatabase();
    }
    
    public async Task<bool> TryAddItemAsync(string itemContent)
    {
        var isAdded = await _database.SetAddAsync(RedisSetKey, itemContent);
        return isAdded;
    }
    
    public async Task DeleteKeyAsync()
    {
        await _database.KeyDeleteAsync(RedisSetKey);
    }
    
}