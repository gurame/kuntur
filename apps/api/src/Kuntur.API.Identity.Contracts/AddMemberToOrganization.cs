using ErrorOr;
using Kuntur.API.Common.UseCases;

namespace Kuntur.API.Identity.Contracts;
public record AddMemberToOrganizationCommand(Guid OrganizationId, Guid UserId) : ICommand<ErrorOr<Success>>;