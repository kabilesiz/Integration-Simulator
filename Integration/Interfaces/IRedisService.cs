namespace Integration.Interfaces;

public interface IRedisService
{
    public Task<bool> TryAddItemAsync(string itemContent);

    public Task DeleteKeyAsync();
}