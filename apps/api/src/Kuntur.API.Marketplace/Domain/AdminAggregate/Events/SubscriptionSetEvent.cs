using Kuntur.API.Common.Domain;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate.Events;
internal record SubscriptionSetEvent(Admin Admin, Subscription Subscription) : IDomainEvent;
