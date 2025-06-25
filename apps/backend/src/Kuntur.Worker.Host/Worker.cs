using Kuntur.Worker.Host.Interfaces;

namespace Kuntur.Worker.Host;

public class Worker(IConsumer consumer, ILogger<Worker> logger) : BackgroundService
{
    private readonly IConsumer _consumer = consumer;
    private readonly ILogger<Worker> _logger = logger;
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await _consumer.ConsumeAsync(ct);
        _logger.LogInformation("Starting integration event consumer background service.");
    }
}
