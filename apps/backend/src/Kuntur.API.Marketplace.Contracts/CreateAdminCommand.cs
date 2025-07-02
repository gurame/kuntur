using ErrorOr;
using Kuntur.API.Shared.UseCases;

namespace Kuntur.API.Marketplace.Contracts;

public record CreateAdminResponse(Guid AdminId);
public record CreateAdminCommand(Guid UserId) : ICommand<ErrorOr<CreateAdminResponse>>;
