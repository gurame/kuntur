using Microsoft.AspNetCore.Routing;

namespace Kuntur.API.Shared.Infrastructure.Endpoints;

public interface IEndpoint
{
    public static abstract void Define(IEndpointRouteBuilder app);
}
