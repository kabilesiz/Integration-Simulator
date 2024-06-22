using System.Collections.Concurrent;
using Integration.Common;
using Integration.Interfaces;

namespace Integration.Service;

public sealed class ItemIntegrationService : IItemIntegrationService
{
    private readonly IItemOperationBackend _itemIntegrationBackend;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();
    private readonly IRedisService _redisService;

    public ItemIntegrationService(IRedisService redisService, IItemOperationBackend itemIntegrationBackend)
    {
        _redisService = redisService;
        _itemIntegrationBackend = itemIntegrationBackend;
    }

    // This is called externally and can be called multithreaded, in parallel.
    // More than one item with the same content should not be saved. However,
    // calling this with different contents at the same time is OK, and should
    // be allowed for performance reasons.
    public async Task<Result> SingleServerScenarioSaveItemAsync(string itemContent)
    {
        var semaphore = _semaphores.GetOrAdd(itemContent, _ => new SemaphoreSlim(1, 1));
        try
        {
            await semaphore.WaitAsync();
            // Check the backend to see if the content is already saved.
            if (_itemIntegrationBackend.FindItemsWithContent(itemContent).Count != 0)
            {
                return new Result(false, $"Duplicate item received with content {itemContent}.");
            }

            var item = _itemIntegrationBackend.SaveItem(itemContent);

            return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
        }
        finally
        {
            semaphore.Release();
        }
    }
    
    public async Task<Result> DistributedSystemScenarioSaveItemAsync(string itemContent)
    {
        bool isAddedToRedis = await _redisService.TryAddItemAsync(itemContent);

        if (!isAddedToRedis)
        {
            return new Result(false, $"Duplicate item received with content '{itemContent}'.");
        }

        var item = _itemIntegrationBackend.SaveItem(itemContent);

        return new Result(true, $"Item with content '{itemContent}' saved with id {item.Id}");
    }

    public List<Item> GetAllItems()
    {
        return _itemIntegrationBackend.GetAllItems();
    }
    
    public async Task DeleteKeyAsync()
    {
        await _redisService.DeleteKeyAsync();
    }
    
    public void ClearItemsList(bool resetAutoIncrement = false)
    {
        _itemIntegrationBackend.ClearListOfItems(resetAutoIncrement);
    }
}