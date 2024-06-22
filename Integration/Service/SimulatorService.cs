using Integration.Common;
using Integration.Helpers;
using Integration.Interfaces;
using Integration.Models;

namespace Integration.Service;

public static class SimulatorService
{
    private static readonly string[] TableColumns = { "ID/INDEX", "CONTENT" };
    public static async Task StartSingleServerScenario(IItemIntegrationService service)
    {
        Console.WriteLine("\tStarting single server scenario...");

        await MenuForDeletingPreviousData(service);
        
        var items = await DataHelper.GetItems();
        var tasks = await GetTasksAsync(items, service.SingleServerScenarioSaveItemAsync);
        
        await Task.WhenAll(tasks);
        
        Console.WriteLine("\tFinished single server scenario");

         await ShowResults(service, items);
    }
    
    public static async Task StartDistributedSystemScenario(IItemIntegrationService service)
    {
        Console.WriteLine("\tStarting distributed system scenario...");
        
        await MenuForDeletingPreviousData(service);
        
        Console.Write("\t--> Would you like to delete previously stored data specific to this application from Redis? [Y/y] : ");
        var response = Console.ReadLine() ?? string.Empty;
        
        if (string.Equals(response, "Y", StringComparison.InvariantCultureIgnoreCase))
        {
            await service.DeleteKeyAsync();
            Console.WriteLine("\t  |--> The previously stored data specific to this application has been deleted from Redis.");
        }
        else
        {
            Console.WriteLine("\t  |--> Continuing with previously stored data specific to this application from Redis.");
        }
        
        var items = await DataHelper.GetItems();
        var tasks = await GetTasksAsync(items, service.DistributedSystemScenarioSaveItemAsync);
       
        await Task.WhenAll(tasks);
        
        Console.WriteLine("\tFinished distributed system scenario");
        
        await ShowResults(service, items);
    }
    
    private static Task<List<Task<Result>>> GetTasksAsync(List<string> items, Func<string, Task<Result>> asyncOperation)
    {
        var tasks = items
            .Select((item,index) =>Task.Run(async () =>
                    {
                        Console.WriteLine($"\t--> CurrentItem : {item} | Id/Index : {index+1}");
                        var result = await asyncOperation(item);
                        Console.WriteLine($"\t--> State : {result.Success} | Message : {result.Message}");
                        return result;
                    }
                )
            )
            .ToList();
        
        return Task.FromResult(tasks);
    }

    private static Task ShowResults(IItemIntegrationService service, List<string> items)
    {
        Console.WriteLine("\tPreparing tables...");
        
        var entryOrder = items
            .Select((item, i) => new TableData {Id = i+1, Content = item})
            .ToList();
        
        var finalOrder = service.GetAllItems()
            .Select((item) => new TableData {Id = item.Id, Content = item.Content})
            .ToList();
        
        TableHelper.ShowResults(entryOrder, nameof(entryOrder).ToUpper(),
            finalOrder.OrderBy(r => r.Id).ToList(), nameof(finalOrder).ToUpper(), TableColumns);
        
        return Task.CompletedTask;
    }
    
    private static Task MenuForDeletingPreviousData(IItemIntegrationService service)
    {
        if (service.GetAllItems().Count <= 0) return Task.CompletedTask;
        
        Console.Write("\t--> Would you like to delete previously stored data ? [Y/y] : ");
        var response = Console.ReadLine() ?? string.Empty;
        
        if (string.Equals(response, "Y", StringComparison.InvariantCultureIgnoreCase))
        {
            Console.Write("\t--> Would you like to reset auto increment id structure(like Truncate Tables SQL Command otherwise Delete From SQL Command) ? [Y/y] : ");
            var autoIncrementResponse = Console.ReadLine() ?? string.Empty;
            
            if (string.Equals(autoIncrementResponse, "Y", StringComparison.InvariantCultureIgnoreCase))
            {
                service.ClearItemsList(true);
            }
            else
            {
                service.ClearItemsList();
            }
            
            Console.WriteLine("\t  |--> The previously stored data has been deleted.");
        }
        else
        {
            Console.WriteLine("\t  |--> Continuing with previously stored data.");
        }

        return Task.CompletedTask;
    }
}