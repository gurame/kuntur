using Kuntur.API.Identity.Contracts;
using Kuntur.API.Identity.Domain.Services;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

namespace Kuntur.API.Identity.UseCases.Profiles;

internal class CreateAdminProfileCommandHandler(IUserService userService) :
    ICommandHandler<CreateAdminProfileCommand, ErrorOr<CreateAdminProfileResponse>>
{
    private readonly IUserService _userService = userService;
    public async Task<ErrorOr<CreateAdminProfileResponse>> Handle(CreateAdminProfileCommand cmd, CancellationToken ct)
    {
        var userId = new UserId(cmd.UserId);
        var result = await _userService.MapRoleAsync(
            userId: userId,
            roles: Roles.From(RoleConstants.MarketplaceAdmin.Name),
            ct);

        if (result.IsError)
        {
            return result.Errors;
        }

        var adminId = result.Value;

        return new CreateAdminProfileResponse(adminId.Value);
    }
}