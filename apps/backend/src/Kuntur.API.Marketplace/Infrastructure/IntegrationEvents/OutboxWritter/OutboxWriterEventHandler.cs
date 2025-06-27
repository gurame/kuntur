using System.Text.Json;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
using Kuntur.API.Marketplace.Infrastructure.Persistence.Outbox;
using Kuntur.SharedKernel.IntegrationEvents;
using Kuntur.SharedKernel.IntegrationEvents.Marketplace;
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
        // TODO: Propagate ActivityContext to the outbox event
        await _outboxRepository.AddAsync(new OutboxIntegrationEvent(
            eventName: integrationEvent.GetType().Name,
            eventContent: JsonSerializer.Serialize(integrationEvent)));
    }
}

