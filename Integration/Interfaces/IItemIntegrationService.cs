using Integration.Common;

namespace Integration.Interfaces;

public interface IItemIntegrationService
{
    public Task<Result> SingleServerScenarioSaveItemAsync(string itemContent);
    
    public Task<Result> DistributedSystemScenarioSaveItemAsync(string itemContent);
    
    public List<Item> GetAllItems();
    
    public Task DeleteKeyAsync();
    
    public void ClearItemsList(bool resetAutoIncrement = false);
}