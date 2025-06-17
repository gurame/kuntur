using Kuntur.API.Identity.Contracts;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.UseCases.Users.Create;

internal class CreateUserCommandHandler(IIdentityProvider provider) : ICommandHandler<CreateUserCommand, ErrorOr<CreateUserResponse>>
{
    private readonly IIdentityProvider _provider = provider;
    public async Task<ErrorOr<CreateUserResponse>> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        var result = await _provider.CreateUserAsync(
            name: new Name(cmd.FirstName, cmd.LastName),
            emailAddress: new EmailAddress(cmd.EmailAddress),
            password: Password.FromPlainText(cmd.Password),
            ct);

        if (result.IsError)
        {
            return result.Errors;
        }

        var userId = result.Value;
        return new CreateUserResponse(userId.Value);
    }
}