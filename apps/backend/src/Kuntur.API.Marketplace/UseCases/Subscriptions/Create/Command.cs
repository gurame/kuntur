using ErrorOr;
using Kuntur.API.Shared.UseCases;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.UseCases.Subscriptions.Create;

internal class CreateSubscriptionCommandHandler(IMarketplaceRepository<Subscription> repository) :
    ICommandHandler<CreateSubscriptionCommand, ErrorOr<CreateSubscriptionResponse>>
{
    private readonly IMarketplaceRepository<Subscription> _repository = repository;
    public async Task<ErrorOr<CreateSubscriptionResponse>> Handle(CreateSubscriptionCommand request, CancellationToken ct)
    {
        // TODO: By default, the subscription type is Free. we should allow the user to choose the subscription type in the future
        var adminId = new AdminId(request.AdminId);
        var subscriptionType = SubscriptionType.Free;
        var subscription = new Subscription(subscriptionType, adminId);

        await _repository.AddAsync(subscription, ct);

        return new CreateSubscriptionResponse(subscription.Id.Value);
    }
}