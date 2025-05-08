using ErrorOr;
using Kuntur.API.Common.UseCases;

namespace Kuntur.API.Marketplace.Contracts;

public record CreateSubscriptionResponse(Guid SubscriptionId);
public record CreateSubscriptionCommand(Guid AdminId) : ICommand<ErrorOr<CreateSubscriptionResponse>>;
