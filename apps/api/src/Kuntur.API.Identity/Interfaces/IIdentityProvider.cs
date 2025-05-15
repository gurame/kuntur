using Kuntur.API.Identity.Domain.OrganizationAggregate.ValueObjects;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

namespace Kuntur.API.Identity.Interfaces;

internal interface IIdentityProvider
{
    Task<ErrorOr<UserId>> CreateUserAsync(Name name, EmailAddress emailAddress, Password password, CancellationToken ct);
    Task<ErrorOr<Success>> DeleteUserAsync(UserId userId, CancellationToken ct);
    Task<ErrorOr<OrganizationId>> CreateOrganizationAsync(string name, CancellationToken ct);
    Task<ErrorOr<Success>> AddMemberToOrganizationAsync(OrganizationId organizationId, UserId userId, CancellationToken ct);
}