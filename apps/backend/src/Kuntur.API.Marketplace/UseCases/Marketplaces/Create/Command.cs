using ErrorOr;
using Kuntur.API.Shared.UseCases;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.UseCases.Marketplaces.Create;

internal class CreateMarketplaceCommandHandler(IMarketplaceRepository<Subscription> repository) :
    ICommandHandler<CreateMarketplaceCommand, ErrorOr<CreateMarketplaceResponse>>
{
    private readonly IMarketplaceRepository<Subscription> _repository = repository;
    public async Task<ErrorOr<CreateMarketplaceResponse>> Handle(CreateMarketplaceCommand request, CancellationToken ct)
    {
        var subscriptionId = new SubscriptionId(request.SubscriptionId);
        var subscription = await _repository.GetByIdAsync(subscriptionId, ct);

        if (subscription is null)
        {
            return SubscriptionErrors.NotFound;
        }

        var marketplace = new MarketplaceAgg(
                taxId: request.TaxtId,
                name: request.Name,
                maxSellers: subscription.GetMaxSellers(),
                subscriptionId: subscription.Id
            );

        var result = subscription.SetMarketplace(marketplace);
        if (result.IsError)
        {
            return result.Errors;
        }

        await _repository.UpdateAsync(subscription, ct);

        return new CreateMarketplaceResponse(marketplace.Id.Value);
    }
}