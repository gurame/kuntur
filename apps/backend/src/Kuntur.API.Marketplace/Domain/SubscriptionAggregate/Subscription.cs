using ErrorOr;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate;
using Kuntur.API.Shared.Domain;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate;

internal class Subscription : AggregateRoot<SubscriptionId>
{
    private readonly int _maxSellers;
    private MarketplaceId? _marketplaceId;

    private Subscription() : base(default!)
    {
    }

    public Subscription(SubscriptionType subscriptionType, AdminId adminId,
        SubscriptionId? subscriptionId = null) : base(subscriptionId ?? new SubscriptionId(Guid.NewGuid()))
    {
        AdminId = adminId;
        SubscriptionType = subscriptionType;
        _maxSellers = GetMaxSellers();
        _domainEvents.Add(new SubscriptionCreatedEvent(this));
    }

    public SubscriptionType SubscriptionType { get; } = null!;
    public AdminId AdminId { get; }

    public ErrorOr<Success> SetMarketplace(TenantMarketplace tenantMarketplace)
    {
        if (_marketplaceId is not null) return SubscriptionErrors.AlreadyHasMarketplaceSet;

        _marketplaceId = tenantMarketplace.Id;

        _domainEvents.Add(new MarketplaceSetEvent(this, tenantMarketplace));
        return Result.Success;
    }

    public int GetMaxSellers()
    {
        return SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 1,
            nameof(SubscriptionType.Starter) => 5,
            nameof(SubscriptionType.Pro) => 20,
            nameof(SubscriptionType.PayAsYouGo) => -1,
            _ => throw new InvalidOperationException()
        };
    }
}