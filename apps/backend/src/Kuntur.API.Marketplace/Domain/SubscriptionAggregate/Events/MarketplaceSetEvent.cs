using Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate;
using Kuntur.API.Shared.Domain;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;

internal record MarketplaceSetEvent(Subscription Subscription, TenantMarketplace TenantMarketplace) : IDomainEvent;