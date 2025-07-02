using Kuntur.API.Shared.DomainEventHandlers;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.DomainEventHandlers.Marketplaces;

internal class MarketplaceSetEventHandler(IMarketplaceRepository<MarketplaceAgg> reposiroty) : IDomainEventHandler<MarketplaceSetEvent>
{
    private readonly IMarketplaceRepository<MarketplaceAgg> _repository = reposiroty;

    public async Task Handle(MarketplaceSetEvent notification, CancellationToken ct)
    {
        await _repository.AddAsync(notification.Marketplace, ct);
    }
}