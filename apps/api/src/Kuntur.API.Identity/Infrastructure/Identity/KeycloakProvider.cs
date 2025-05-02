using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.Infrastructure.Identity;

internal class KeycloakProvider : IIdentityProvider
{
    public Task<ErrorOr<UserId>> CreateUserAsync(string email, string password, CancellationToken cancellationToken)
    {
       return Task.FromResult<ErrorOr<UserId>>(new UserId(Guid.NewGuid()));
    }

    public Task<ErrorOr<bool>> DeleteUserAsync(UserId userId, CancellationToken cancellationToken)
    {
        return Task.FromResult<ErrorOr<bool>>(true);
    }
}
