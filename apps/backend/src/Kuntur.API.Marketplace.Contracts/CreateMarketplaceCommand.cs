using ErrorOr;
using Kuntur.API.Shared.UseCases;

namespace Kuntur.API.Marketplace.Contracts;

public record CreateMarketplaceResponse(Guid MarketplaceId);
public record CreateMarketplaceCommand(Guid MarketplaceId, Guid SubscriptionId, 
    string TaxtId, string Name) : ICommand<ErrorOr<CreateMarketplaceResponse>>;
