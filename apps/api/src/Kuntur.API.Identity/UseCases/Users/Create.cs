using Kuntur.API.Identity.Contracts;
using Kuntur.API.Identity.Domain.Services;

namespace Kuntur.API.Identity.UseCases.Users;

internal class CreateUserCommandHandler(IUserService userService) : ICommandHandler<CreateUserCommand, ErrorOr<CreateUserResponse>>
{
    private readonly IUserService _userService = userService;
    public async Task<ErrorOr<CreateUserResponse>> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        var result = await _userService.CreateUserAsync(
            cmd.FirstName,
            cmd.LastName,
            cmd.EmailAddress,
            cmd.PhoneNumber,
            cmd.Password,
            ct);

        if (result.IsError)
        {
            return result.Errors;
        }

        var userId = result.Value;
        return new CreateUserResponse(userId.Value);
    }
}