using ErrorOr;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate;

public static class SubscriptionErrors
{
    public static readonly Error NotFound
        = Error.NotFound(code: "Subscription.NotFound", description: "Subscription not found");
    public static readonly Error AlreadyHasMarketplaceSet
        = Error.Conflict(code: "Subscription.AlreadyHasMarketplaceSet", description: "Subscription already has marketplace set");
}