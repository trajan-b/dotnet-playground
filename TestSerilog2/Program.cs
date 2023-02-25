using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

var builder = Host.CreateDefaultBuilder()
    .ConfigureLogging((context, loggingBuilder) =>
    {
        var serilogLogger = new LoggerConfiguration()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.FromLogContext()
            .MinimumLevel.Warning()
            .WriteTo.Console()
            .CreateLogger();

        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(serilogLogger);
    })
    .ConfigureServices((context, collection) =>
    {
        collection.AddHostedService<Worker>();
    });

    
var app = builder.Build();

app.Run();

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogWarning("Thread id 1 : {ThreadId}");
        await Test();
    }

    private async Task Test()
    {
        _logger.LogWarning("Thread id 2 : {ThreadId}");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        
    }
}