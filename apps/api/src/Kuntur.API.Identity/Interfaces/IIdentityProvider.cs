using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

namespace Kuntur.API.Identity.Interfaces;

internal interface IIdentityProvider
{
    Task<ErrorOr<UserId>> CreateUserAsync(EmailAddress emailAddress, string password, CancellationToken ct);
    Task<ErrorOr<Success>> DeleteUserAsync(UserId userId, CancellationToken ct);
}