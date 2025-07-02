using ErrorOr;
using Kuntur.API.Shared.UseCases;

namespace Kuntur.API.Marketplace.Contracts;

public record CreateSubscriptionResponse(Guid SubscriptionId);
public record CreateSubscriptionCommand(Guid AdminId) : ICommand<ErrorOr<CreateSubscriptionResponse>>;
