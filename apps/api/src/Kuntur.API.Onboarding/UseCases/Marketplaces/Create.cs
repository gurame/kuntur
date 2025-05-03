using Kuntur.API.Common.Infrastructure.Endpoints;

namespace Kuntur.API.Onboarding.UseCases.Marketplaces;
public class Endpoint : IEndpoint
{
    public record Request(
        string Name, string TaxId,
        string FirstName, string LastName,
        string PhoneNumber, string EmailAddress, string Password);
    public record Response(Guid MarketplaceId);
    public static void Define(IEndpointRouteBuilder app)
    {
        app.MapPost(Routes.Create, async (ISender sender, Request request) =>
        {
            var response = await sender.Send(new Command(
                request.Name, request.TaxId,
                request.FirstName, request.LastName,
                request.PhoneNumber, request.EmailAddress, request.Password));

            return TypedResults.Ok(response);
        })
        .Produces<Response>(StatusCodes.Status200OK)
        .MapToApiVersion(1, 0);
    }
}
