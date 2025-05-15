using Kuntur.API.Identity.Contracts;
using Kuntur.API.Identity.Domain.OrganizationAggregate.ValueObjects;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.UseCases.Organizations;

internal class AddMemberToOrganizationCommandHandler(IIdentityProvider identityProvider) :
    ICommandHandler<AddMemberToOrganizationCommand, ErrorOr<Success>>
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<ErrorOr<Success>> Handle(AddMemberToOrganizationCommand cmd, CancellationToken ct)
    {
        var result = await _identityProvider.AddMemberToOrganizationAsync(
            organizationId: new OrganizationId(cmd.OrganizationId),
            userId: new UserId(cmd.UserId),
            ct);

        if (result.IsError)
        {
            return result.Errors;
        }

        return Result.Success;
    }
}