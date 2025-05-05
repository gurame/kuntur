using Kuntur.API.Common.Infrastructure.Endpoints;

namespace Kuntur.API.Onboarding.UseCases.Marketplaces;
public class CreateEndpoint : IEndpoint
{
    public record CreateRequest(
        string Name, string TaxId,
        string FirstName, string LastName,
        string PhoneNumber, string EmailAddress, string Password);
    public record CreateResponse(Guid MarketplaceId);
    public static void Define(IEndpointRouteBuilder app)
    {
        app.MapPost(Routes.Create, async (ISender sender, CreateRequest request) =>
        {
            var cmd = new CreateCommand(
                request.Name, request.TaxId,
                request.FirstName, request.LastName,
                request.EmailAddress, request.PhoneNumber,
                request.Password);

            var result = await sender.Send(cmd);

            return result.MapResponse<CreateCommandResult, CreateResponse>();
        })
        .Produces<CreateResponse>(StatusCodes.Status200OK)
        .MapToApiVersion(1, 0);
    }
}
