using Kuntur.API.Common.Infrastructure.IntegrationEvents.Marketplace;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kuntur.Worker.Notifications.Marketplace;

public class MarketplaceCreatedEventHandler(ILogger<MarketplaceCreatedEventHandler> logger) 
    : INotificationHandler<MarketplaceCreatedIntegrationEvent>
{
    private readonly ILogger<MarketplaceCreatedEventHandler> _logger = logger;
    public async Task Handle(MarketplaceCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling MarketplaceCreatedIntegrationEvent for Marketplace ID: {MarketplaceId}, Name: {MarketplaceName}",
            notification.MarketplaceId, notification.Name);

        await Task.Delay(1000, cancellationToken); // Simulating async work
        _logger.LogInformation("MarketplaceCreatedIntegrationEvent processed successfully.");
    }
}
