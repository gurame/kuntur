
using Kuntur.API.Common.DomainEventHandlers;
using Kuntur.API.Marketplace.Domain.AdminAggregate.Events;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.DomainEventHandlers.Subscriptions;

internal class SubscriptionSetEventHandler(IMarketplaceRepository<Subscription> reposiroty) : IDomainEventHandler<SubscriptionSetEvent>
{
    private readonly IMarketplaceRepository<Subscription> _repository = reposiroty;

    public async Task Handle(SubscriptionSetEvent notification, CancellationToken ct)
    {
        await _repository.AddAsync(notification.Subscription, ct);
    }
}