using Kuntur.API.Common.Domain;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
internal record SubscriptionCreatedEvent(Subscription Subscription) : IDomainEvent;
