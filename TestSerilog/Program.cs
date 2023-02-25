
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

var builder = Host.CreateDefaultBuilder()
    .ConfigureLogging((context, loggingBuilder) =>
    {
        var serilogLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Warning()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {MessageId} {Message:lj}{NewLine}{Exception}")
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
        var messageId = Guid.NewGuid();
        
        using var property = LogContext.PushProperty("MessageId", messageId);
        
        _logger.LogWarning("Test 1");
        
        Test();
    }

    private void Test()
    {
        _logger.LogWarning("Test 2");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        
    }
}
    
    