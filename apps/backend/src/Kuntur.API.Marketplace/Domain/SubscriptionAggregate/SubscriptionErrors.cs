using ErrorOr;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate;

public static class SubscriptionErrors
{
    public static readonly Error NotFound
        = Error.NotFound("Subscription.NotFound", "Subscription not found");

    public static readonly Error AlreadyHasMarketplaceSet
        = Error.Conflict("Subscription.AlreadyHasMarketplaceSet", "Subscription already has marketplace set");
}