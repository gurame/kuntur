using Kuntur.API.Common.Domain;
using Kuntur.API.Marketplace.Domain.AdminAggregate.Events;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;
using Throw;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate;

internal class Admin : AggregateRoot<AdminId>
{
    public UserId UserId { get; }
    public SubscriptionId? SubscriptionId { get; private set; }

    private Admin() : base(default!) { }
    public Admin(AdminId adminId, UserId userId) : base(adminId)
    {
        UserId = userId;
    }

    public void SetSubscription(Subscription subscription)
    {
        SubscriptionId.HasValue.Throw().IfTrue();

        SubscriptionId = subscription.Id;

        _domainEvents.Add(new SubscriptionSetEvent(this, subscription));
    }
}