using ErrorOr;
using Kuntur.API.Common.UseCases;

namespace Kuntur.API.Marketplace.Contracts;

public record CreateAdminResponse(Guid AdminId);
public record CreateAdminCommand(Guid UserId, Guid AdminId) : ICommand<ErrorOr<CreateAdminResponse>>;
