using System.Text.Json;
using Kuntur.API.Common.Infrastructure.IntegrationEvents;
using Kuntur.API.Common.Infrastructure.IntegrationEvents.Marketplace;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
using Kuntur.API.Marketplace.Infrastructure.Persistence;
using Kuntur.API.Marketplace.Infrastructure.Persistence.Outbox;
using MediatR;

namespace Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.OutboxWritter;

internal class OutboxWriterEventHandler(IOutboxRepository outboxRepository) :
    INotificationHandler<MarketplaceSetEvent>
{
    private readonly IOutboxRepository _outboxRepository = outboxRepository;
    public async Task Handle(MarketplaceSetEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new MarketplaceCreatedIntegrationEvent(
            notification.Marketplace.Id.Value, notification.Marketplace.Name);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent)
    {
        await _outboxRepository.AddAsync(new OutboxIntegrationEvent(
            eventName: integrationEvent.GetType().Name,
            eventContent: JsonSerializer.Serialize(integrationEvent)));
    }
}

