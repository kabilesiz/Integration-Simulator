using BenchmarkDotNet.Running;
using Integration.Backend;
using Integration.Helpers;
using Integration.Interfaces;
using Integration.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        const string redisConnectionString = "localhost:6379";
        
        
        services.AddScoped(_ => new RedisService(redisConnectionString));
        services.AddScoped<IRedisService>(provider => provider.GetService<RedisService>());
        services.AddScoped<IItemOperationBackend, ItemOperationBackend>();
        services.AddScoped<IItemIntegrationService, ItemIntegrationService>();
        
        var serviceProvider = services.BuildServiceProvider();
        var itemIntegrationService = serviceProvider.GetRequiredService<IItemIntegrationService>();

        while (true)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Which scenario would you like to continue with ?: ");
                Console.WriteLine("\t--> 1: Single Server Scenario");
                Console.WriteLine("\t--> 2: Distributed System Scenario");
                Console.WriteLine("\t--> 3: Benchmark Tests (Make sure you are in release mode to ensure reliable results)");
                Console.Write("Please enter the number corresponding to the operation you wish to perform : ");
                var scenarioNumber = int.Parse(Console.ReadLine() ?? string.Empty);
                
                Func<Task> action = scenarioNumber switch
                {
                    1 => () => SimulatorService.StartSingleServerScenario(itemIntegrationService),
                    2 => () => SimulatorService.StartDistributedSystemScenario(itemIntegrationService),
                    3 => DoBenchmarkTests,
                    _ => DataHelper.Error
                };
        
                await action();

                await DataHelper.AgainMenu();
            }
            catch (Exception)
            {
                await DataHelper.Error();
            }
        }
    }

    private static Task DoBenchmarkTests()
    {
        var config = new BenchmarkConfig();
        BenchmarkRunner.Run<Benchmarks>(config);
        
        return Task.CompletedTask;
    }
}