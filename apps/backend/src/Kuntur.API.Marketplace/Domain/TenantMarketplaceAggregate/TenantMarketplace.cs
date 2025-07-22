using Kuntur.API.Marketplace.Domain.MarketplaceAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;
using Kuntur.API.Shared.Domain;

namespace Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate;

internal class TenantMarketplace : AggregateRoot<MarketplaceId>
{
    private readonly int _maxSellers;
    private readonly SubscriptionId _subscriptionId;

    private TenantMarketplace() : base(default!)
    {
    }

    public TenantMarketplace(string taxId, string name, int maxSellers,
        SubscriptionId subscriptionId, MarketplaceId? id = null) : base(id ?? new MarketplaceId(Guid.NewGuid()))
    {
        TaxId = taxId;
        Name = name;
        _maxSellers = maxSellers;
        _subscriptionId = subscriptionId;
    }

    public string TaxId { get; } = default!;

    public string Name { get; } = default!;
}