using ErrorOr;
using Kuntur.API.Common.Domain;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate;

internal class Admin : AggregateRoot<AdminId>
{
    public UserId UserId { get; }
    public SubscriptionId? SubscriptionId { get; private set; }

    private Admin() : base(default!) { }
    public Admin(UserId userId, AdminId? id = null) : base(id ?? new AdminId(Guid.NewGuid()))
    {
        UserId = userId;
    }
    public ErrorOr<Success> SetSubscription(Subscription subscription)
    {
        if (SubscriptionId is not null)
        {
            return AdminErrors.AlreadyHasSubscriptionSet;
        }

        SubscriptionId = subscription.Id;
        return Result.Success;
    }
}