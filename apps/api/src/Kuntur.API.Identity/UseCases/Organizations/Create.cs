using Kuntur.API.Identity.Contracts;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.UseCases.Organizations;

internal class CreateOrganizationCommandHandler(IIdentityProvider identityProvider) : 
    ICommandHandler<CreateOrganizationCommand, ErrorOr<CreateOrganizationResponse>>
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<ErrorOr<CreateOrganizationResponse>> Handle(CreateOrganizationCommand cmd, CancellationToken ct)
    {
        var result = await _identityProvider.CreateOrganizationAsync(
            cmd.Name,
            ct);

        if (result.IsError)
        {
            return result.Errors;
        }

        var organizationId = result.Value;
        return new CreateOrganizationResponse(organizationId.Value);
    }
}