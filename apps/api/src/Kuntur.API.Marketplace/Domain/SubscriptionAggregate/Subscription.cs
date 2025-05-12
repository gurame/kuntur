using ErrorOr;
using Kuntur.API.Common.Domain;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate;

internal class Subscription : AggregateRoot<SubscriptionId>
{
    public SubscriptionType SubscriptionType { get; } = null!;
    private readonly int _maxSellers;
    private readonly AdminId _adminId;
    public AdminId AdminId => _adminId;
    private MarketplaceId? _marketplaceId;
    private Subscription() : base(default!) { }
    public Subscription(SubscriptionType subscriptionType, AdminId adminId,
        SubscriptionId? subscriptionId = null) : base(subscriptionId ?? new SubscriptionId(Guid.NewGuid()))
    {
        _adminId = adminId;
        SubscriptionType = subscriptionType;
        _maxSellers = GetMaxSellers();
        _domainEvents.Add(new SubscriptionCreatedEvent(this));
    }

    public ErrorOr<Success> SetMarketplace(MarketplaceAgg marketplace)
    {
        if (_marketplaceId is not null)
        {
            return SubscriptionErrors.AlreadyHasMarketplaceSet;
        }

        _marketplaceId = marketplace.Id;

        _domainEvents.Add(new MarketplaceSetEvent(this, marketplace));
        return Result.Success;
    }

    public int GetMaxSellers() => SubscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 1,
        nameof(SubscriptionType.Starter) => 5,
        nameof(SubscriptionType.Pro) => 20,
        _ => throw new InvalidOperationException()
    };
}