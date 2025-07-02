using ErrorOr;
using Kuntur.API.Shared.Domain.EventualConsistency;
using Kuntur.API.Shared.DomainEventHandlers;
using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.Events;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.DomainEventHandlers.Admins;

internal class SubscriptionCreatedEventHandler(IMarketplaceRepository<Admin> repository) :
    IDomainEventHandler<SubscriptionCreatedEvent>
{
    private readonly IMarketplaceRepository<Admin> _repository = repository;
    public async Task Handle(SubscriptionCreatedEvent notification, CancellationToken ct)
    {
        var admin = await _repository.GetByIdAsync(notification.Subscription.AdminId, ct) ?? 
            throw new EventualConsistencyException(Error.Conflict(
                $"Admin with id {notification.Subscription.AdminId} not found"));

        var result = admin.SetSubscription(notification.Subscription);
        if (result.IsError)
        {
            throw new EventualConsistencyException(result.Errors[0]);
        }

        await _repository.UpdateAsync(admin, ct);
    }
}