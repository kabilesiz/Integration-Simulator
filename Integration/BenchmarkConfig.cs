using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using Perfolizer.Horology;

namespace Integration;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.Default
            .WithWarmupCount(5)
            .WithIterationCount(20)
            .WithIterationTime(TimeInterval.FromMilliseconds(200))); 
        
        AddLogger(ConsoleLogger.Default);
        
        AddExporter(CsvExporter.Default);
        AddExporter(HtmlExporter.Default);
        AddExporter(MarkdownExporter.Default);
        
        AddColumnProvider(DefaultColumnProviders.Instance);
    }
}