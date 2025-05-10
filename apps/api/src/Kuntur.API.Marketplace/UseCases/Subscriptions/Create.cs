using ErrorOr;
using Kuntur.API.Common.UseCases;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.UseCases.Subscriptions;

internal class CreateSubscriptionCommandHandler(IMarketplaceRepository<Admin> repository) :
    ICommandHandler<CreateSubscriptionCommand, ErrorOr<CreateSubscriptionResponse>>
{
    private readonly IMarketplaceRepository<Admin> _repository = repository;
    public async Task<ErrorOr<CreateSubscriptionResponse>> Handle(CreateSubscriptionCommand request, CancellationToken ct)
    {
        var adminId = new AdminId(request.AdminId);
        var admin = await _repository.GetByIdAsync(adminId, ct);

        if (admin is null)
        {
            return DomainErrors.Admin.NotFound;
        }

        // TODO: By default, the subscription type is Free. we should allow the user to choose the subscription type in the future
        var subscriptionType = SubscriptionType.Free;
        var subscription = new Subscription(subscriptionType, adminId);
        admin.SetSubscription(subscription);

        await _repository.UpdateAsync(admin, ct);

        return new CreateSubscriptionResponse(subscription.Id.Value);
    }
}