using Kuntur.API.Common.Domain;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate;

internal class Subscription : AggregateRoot<SubscriptionId>
{
    public SubscriptionType SubscriptionType { get; } = null!;
    private readonly int _maxSellers;
    private readonly Guid _adminId;
    private Subscription() : base(default!) { }
    public Subscription(SubscriptionType subscriptionType, AdminId adminId,
        SubscriptionId? subscriptionId = null) : base(subscriptionId ?? new SubscriptionId(Guid.NewGuid()))
    {
        _adminId = adminId.Value;
        SubscriptionType = subscriptionType;
        _maxSellers = GetMaxSellers();
    }
    private int GetMaxSellers() => SubscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 1,
        nameof(SubscriptionType.Starter) => 5,
        nameof(SubscriptionType.Pro) => 20,
        _ => throw new InvalidOperationException()
    };
}