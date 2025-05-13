using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

namespace Kuntur.API.Identity.Interfaces;

internal interface IIdentityProvider
{
    Task<ErrorOr<UserId>> CreateUserAsync(EmailAddress emailAddress, Password password, CancellationToken ct);
    Task<ErrorOr<Success>> MapRoleToUserAsync(UserId userId, Roles roles, CancellationToken ct);
    Task<ErrorOr<Success>> DeleteUserAsync(UserId userId, CancellationToken ct);
}