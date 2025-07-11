using Kuntur.API.Shared.Domain;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;

namespace Kuntur.API.Marketplace.Domain.MarketplaceAggregate;
internal class MarketplaceAgg : AggregateRoot<MarketplaceId>
{
    private readonly string _taxId = default!;
    public string TaxId => _taxId;
    private readonly string _name = default!;
    public string Name => _name;
    private readonly int _maxSellers = default!;
    private readonly SubscriptionId _subscriptionId = default!;
    private MarketplaceAgg() : base(default!) { }
    public MarketplaceAgg(string taxId, string name, int maxSellers, 
        SubscriptionId subscriptionId, MarketplaceId? id = null) : base(id ?? new MarketplaceId(Guid.NewGuid()))
    {
        _taxId = taxId;
        _name = name;
        _maxSellers = maxSellers;
        _subscriptionId = subscriptionId;
    }
}