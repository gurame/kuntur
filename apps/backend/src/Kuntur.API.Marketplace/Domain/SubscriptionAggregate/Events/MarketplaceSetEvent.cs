using Kuntur.API.Shared.Domain;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
internal record MarketplaceSetEvent(Subscription Subscription, MarketplaceAgg Marketplace) : IDomainEvent;
