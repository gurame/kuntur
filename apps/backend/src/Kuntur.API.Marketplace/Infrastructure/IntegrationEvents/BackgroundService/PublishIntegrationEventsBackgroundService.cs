using System.Text.Json;
using Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.IntegrationEventsPublisher;
using Kuntur.API.Marketplace.Infrastructure.Persistence.Outbox;
using Kuntur.SharedKernel.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Throw;

namespace Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.BackgroundService;

internal class PublishIntegrationEventsBackgroundService(
    IIntegrationEventsPublisher integrationEventPublisher,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<PublishIntegrationEventsBackgroundService> logger) : IHostedService
{
    private readonly CancellationTokenSource _cts = new();
    private readonly IIntegrationEventsPublisher _integrationEventPublisher = integrationEventPublisher;
    private readonly ILogger<PublishIntegrationEventsBackgroundService> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private Task? _doWorkTask;
    private PeriodicTimer? _timer;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _integrationEventPublisher.InitializeAsync(cancellationToken);
        _doWorkTask = DoWorkAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_doWorkTask is null) return;

        _cts.Cancel();
        await _doWorkTask;

        _timer?.Dispose();
        _cts.Dispose();
    }

    private async Task DoWorkAsync()
    {
        _logger.LogInformation("Starting integration event publisher background service.");

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (await _timer.WaitForNextTickAsync(_cts.Token))
            try
            {
                await PublishIntegrationEventsFromDbAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred while publishing integration events.");
            }
    }

    private async Task PublishIntegrationEventsFromDbAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        var outboxIntegrationEvents = await repository.GetPendingEventsAsync();
        if (outboxIntegrationEvents.Count > 0)
        {
            _logger.LogInformation("Read a total of {NumEvents} outbox integration events",
                outboxIntegrationEvents.Count);
            outboxIntegrationEvents.ForEach(outboxIntegrationEvent =>
            {
                var integrationEvent =
                    JsonSerializer.Deserialize<IIntegrationEvent>(outboxIntegrationEvent.EventContent);
                integrationEvent.ThrowIfNull();

                _logger.LogInformation("Publishing event of type: {EventType}", integrationEvent.GetType().Name);
                _integrationEventPublisher.PublishEventAsync(integrationEvent);
                _logger.LogInformation("Integration event published successfully");
            });

            await repository.RemoveRangeAsync(outboxIntegrationEvents);
        }
    }
}