using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
using Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate;
using Kuntur.API.Marketplace.Interfaces;
using Kuntur.API.Shared.DomainEventHandlers;

namespace Kuntur.API.Marketplace.DomainEventHandlers.Marketplaces;

internal class MarketplaceSetEventHandler(IMarketplaceRepository<TenantMarketplace> repository)
    : IDomainEventHandler<MarketplaceSetEvent>
{
    private readonly IMarketplaceRepository<TenantMarketplace> _repository = repository;

    public async Task Handle(MarketplaceSetEvent notification, CancellationToken ct)
    {
        await _repository.AddAsync(notification.TenantMarketplace, ct);
    }
}