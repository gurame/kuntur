using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

namespace Kuntur.API.Identity.Interfaces;

internal interface IIdentityProvider
{
    Task<ErrorOr<UserId>> CreateUserAsync(EmailAddress emailAddress, string password, CancellationToken cancellationToken);
    Task<ErrorOr<bool>> DeleteUserAsync(UserId userId, CancellationToken cancellationToken);
}