using BenchmarkDotNet.Attributes;
using Integration.Backend;
using Integration.Service;

namespace Integration;
public class Benchmarks
{
     private const string RedisConnectionString = "localhost:6379";
     private readonly RedisService _redisService = new(RedisConnectionString);
     private readonly ItemOperationBackend _itemOperationBackend = new();
     private readonly ItemIntegrationService _itemIntegrationService;

     private const string LongUniqueString = "vitae, neque, quam, libero, modi, sint, sed, quos, eveniet, et, harum, consequatur, reprehenderit, velit, nihil, enim, ut, exercitationem, numquam, fugiat, hic, soluta, rerum, corporis, odit, nostrum, facere, qui, nisi, omnis, autem, ea, fugit, occaecati, quo, adipisci, vel, ipsa, maxime, eos, facilis, voluptatibus, voluptates, voluptas, ducimus, consectetur, aut, est, rem, doloremque, delectus, laboriosam, magni, dolor, ab, minima, natus, cupiditate, nemo, alias, quia, distinctio, commodi, beatae, praesentium, incidunt, non, molestias, iste, debitis, suscipit, impedit, perspiciatis, reiciendis, doloribus, assumenda, error, nobis, in, quis, laudantium, repudiandae, tenetur, molestiae, expedita, voluptate, iure, similique, illo, porro, cum, dolorum, sit, dignissimos, amet, earum, vero, esse, aliquam, recusandae, eligendi, nam, voluptatem, ratione, quae, nesciunt, accusamus, architecto, excepturi, quidem, atque, aperiam, odio, itaque, eum, unde, a, deserunt, tempore, voluptatum, aspernatur, temporibus, ipsum, quasi, accusantium, nulla, asperiores, provident, quaerat, dolorem, laborum, eaque, sapiente, placeat, dicta, dolores, quod, totam, animi, possimus, necessitatibus, minus, perferendis, sunt, tempora, aliquid, officiis, dolore, mollitia, magnam, cumque, culpa, repellat, officia, at, consequuntur, illum, fuga, id, labore, iusto";
     private const string Long25PercentDuplicateString = "AdCreativeAI AdCreativeAI AdCreativeAI repellat hic tempora AdCreativeAI nam neque ut neque et repellendus reiciendis dignissimos qui doloremque expedita blanditiis possimus voluptatem AdCreativeAI AdCreativeAI voluptas molestias odit sit AdCreativeAI nemo nihil adipisci AdCreativeAI ut nam doloribus id culpa AdCreativeAI consequuntur dolorum quia asperiores impedit quaerat eaque aut sed AdCreativeAI dolor AdCreativeAI qui natus sint officia rerum AdCreativeAI ut quos AdCreativeAI aut AdCreativeAI beatae minima AdCreativeAI nam asperiores AdCreativeAI AdCreativeAI distinctio saepe sed non est quia est reiciendis AdCreativeAI voluptas facilis reiciendis culpa sequi eligendi dolor tempore AdCreativeAI libero sequi aut AdCreativeAI AdCreativeAI AdCreativeAI est ab odit velit AdCreativeAI vero dolore cupiditate AdCreativeAI fugiat ea laudantium AdCreativeAI molestiae et sequi AdCreativeAI AdCreativeAI AdCreativeAI amet AdCreativeAI AdCreativeAI AdCreativeAI veniam ea AdCreativeAI AdCreativeAI AdCreativeAI cumque aut facere ut AdCreativeAI debitis AdCreativeAI voluptatem inventore repellat itaque dolor vel AdCreativeAI AdCreativeAI nihil iste ullam natus cumque AdCreativeAI odio ratione sit accusamus aut error ab voluptate aut perspiciatis laborum AdCreativeAI ipsa eveniet qui qui sed rem asperiores amet";
     private const string Long50PercentDuplicateString = "AdCreativeAI porro facilis est AdCreativeAI ut soluta AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI numquam alias AdCreativeAI distinctio eligendi AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI nihil ex AdCreativeAI AdCreativeAI quis minima AdCreativeAI accusamus AdCreativeAI impedit aut eligendi AdCreativeAI assumenda AdCreativeAI et quo illo magni quae modi vitae AdCreativeAI qui AdCreativeAI AdCreativeAI rerum AdCreativeAI voluptas eius dolorem AdCreativeAI voluptatem AdCreativeAI AdCreativeAI AdCreativeAI deserunt at placeat AdCreativeAI at AdCreativeAI et iure AdCreativeAI ipsa AdCreativeAI AdCreativeAI AdCreativeAI harum laudantium dignissimos AdCreativeAI enim AdCreativeAI AdCreativeAI a AdCreativeAI facilis voluptate AdCreativeAI AdCreativeAI quia AdCreativeAI autem a AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI facere AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI illo officiis est AdCreativeAI AdCreativeAI aliquid AdCreativeAI necessitatibus sint velit ad AdCreativeAI et AdCreativeAI eligendi AdCreativeAI ut AdCreativeAI AdCreativeAI AdCreativeAI quod AdCreativeAI AdCreativeAI accusantium AdCreativeAI AdCreativeAI blanditiis AdCreativeAI dolore AdCreativeAI minima qui AdCreativeAI AdCreativeAI deleniti tenetur corporis AdCreativeAI aut AdCreativeAI aliquid AdCreativeAI illo aut nam AdCreativeAI AdCreativeAI consectetur officiis id enim dolore AdCreativeAI AdCreativeAI doloremque expedita AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI";
     private const string Long75PercentDuplicateString = "AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI atque AdCreativeAI excepturi AdCreativeAI eum AdCreativeAI AdCreativeAI AdCreativeAI blanditiis AdCreativeAI AdCreativeAI eligendi AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI at AdCreativeAI AdCreativeAI laudantium AdCreativeAI AdCreativeAI magnam AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI quae AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI eos AdCreativeAI AdCreativeAI omnis AdCreativeAI AdCreativeAI nobis AdCreativeAI beatae AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI tempora dolores AdCreativeAI quibusdam AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI qui AdCreativeAI asperiores AdCreativeAI AdCreativeAI expedita AdCreativeAI a AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI saepe AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI voluptas AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI sed AdCreativeAI laboriosam et AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI vitae eos AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI rem AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI quo eius at atque in AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI voluptatum AdCreativeAI eius AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI impedit AdCreativeAI AdCreativeAI et AdCreativeAI beatae AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI non maxime ab AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI";
     private const string LongFullDuplicateString = "AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI AdCreativeAI";
     
     private readonly List<string> _uniqueItems = new(LongUniqueString.Split(' '));
     private readonly List<string> _atLeast25PercentItems = new(Long25PercentDuplicateString.Split(' '));
     private readonly List<string> _atLeast50PercentDuplicateItems = new(Long50PercentDuplicateString.Split(' '));
     private readonly List<string> _atLeast75PercentDuplicateItems = new(Long75PercentDuplicateString.Split(' '));
     private readonly List<string> _fullDuplicateItems = new(LongFullDuplicateString.Split(' '));
     private const int _iterationNumber = 100;

     public Benchmarks()
     {
         _itemIntegrationService = new ItemIntegrationService(_redisService, _itemOperationBackend);
     }
     
     [Benchmark]
     public async Task SingleServerScenario_WithFullUniqueItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _uniqueItems
                 .Select(currentData =>Task.Run(async () =>
                         {
                             await _itemIntegrationService.SingleServerScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task DistributedSystemScenario_WithFullUniqueItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
              var tasks = _uniqueItems
                 .Select(currentData => Task.Run(async () =>
                         {
                             await _itemIntegrationService.DistributedSystemScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
              
              await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task SingleServerScenario_With25PercentDuplicateItem()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _atLeast25PercentItems
                 .Select(currentData =>Task.Run(async () =>
                         {
                             await _itemIntegrationService.SingleServerScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task DistributedSystemScenario_With25PercentDuplicateItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _atLeast25PercentItems
                 .Select(currentData => Task.Run(async () =>
                         {
                             await _itemIntegrationService.DistributedSystemScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task SingleServerScenario_With50PercentDuplicateItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _atLeast50PercentDuplicateItems
                 .Select(currentData =>Task.Run(async () =>
                         {
                             await _itemIntegrationService.SingleServerScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task DistributedSystemScenario_With50PercentDuplicateItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _atLeast50PercentDuplicateItems
                 .Select(currentData => Task.Run(async () =>
                         {
                             await _itemIntegrationService.DistributedSystemScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task SingleServerScenario_With75PercentDuplicateItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _atLeast75PercentDuplicateItems
                 .Select(currentData =>Task.Run(async () =>
                         {
                             await _itemIntegrationService.SingleServerScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task DistributedSystemScenario_With75PercentDuplicateItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _atLeast75PercentDuplicateItems
                 .Select(currentData => Task.Run(async () =>
                         {
                             await _itemIntegrationService.DistributedSystemScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task SingleServerScenario_WithFullDuplicateItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _fullDuplicateItems
                 .Select(currentData =>Task.Run(async () =>
                         {
                             await _itemIntegrationService.SingleServerScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
     
     [Benchmark]
     public async Task DistributedSystemScenario_WithFullDuplicateItems()
     {
         for (var i = 0; i < _iterationNumber; i++)
         {
             var tasks = _fullDuplicateItems
                 .Select(currentData => Task.Run(async () =>
                         {
                             await _itemIntegrationService.DistributedSystemScenarioSaveItemAsync(currentData);
                         }
                     )
                 )
                 .ToList();
             
             await Task.WhenAll(tasks);
         }
     }
}