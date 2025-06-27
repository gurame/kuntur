using Kuntur.SharedKernel.IntegrationEvents;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence.Outbox;

internal interface IOutboxRepository
{
    Task AddAsync(OutboxIntegrationEvent outboxIntegrationEvent, CancellationToken cancellationToken = default);
    Task<List<OutboxIntegrationEvent>> GetPendingEventsAsync(CancellationToken cancellationToken = default);
    Task RemoveRangeAsync(IEnumerable<OutboxIntegrationEvent> outboxIntegrationEvents, CancellationToken cancellationToken = default);
}