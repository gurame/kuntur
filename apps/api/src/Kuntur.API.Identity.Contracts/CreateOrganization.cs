using ErrorOr;
using Kuntur.API.Common.UseCases;

namespace Kuntur.API.Identity.Contracts;
public record CreateOrganizationResponse(Guid OrganizationId);
public record CreateOrganizationCommand(string Name) : ICommand<ErrorOr<CreateOrganizationResponse>>;