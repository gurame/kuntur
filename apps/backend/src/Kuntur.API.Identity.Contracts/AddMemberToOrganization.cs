using ErrorOr;
using Kuntur.API.Shared.UseCases;

namespace Kuntur.API.Identity.Contracts;
public record AddMemberToOrganizationCommand(Guid OrganizationId, Guid UserId) : ICommand<ErrorOr<Success>>;