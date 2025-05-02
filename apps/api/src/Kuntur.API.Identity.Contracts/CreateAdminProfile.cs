using Kuntur.API.Common.Domain.ValueObjects;
using Kuntur.API.Common.UseCases;

namespace Kuntur.API.Identity.Contracts;

public record CreateAdminProfileResponse(Guid AdminId);
public record CreateAdminProfileCommand(Guid UserId) : ICommand<CreateAdminProfileResponse>;

