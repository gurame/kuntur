using Kuntur.API.Shared.Domain;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
internal record SubscriptionCreatedEvent(Subscription Subscription) : IDomainEvent;
