using Kuntur.API.Shared;
using Kuntur.API.Shared.Infrastructure.Endpoints;
using Kuntur.API.Identity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kuntur.API.Identity;

public class Configuration : IModuleConfiguration
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services.AddInfrastructure(configuration);
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        const string basePath = "identity";
        var group = app.MapGroup(basePath);
        group.MapEndopints<Configuration>();
    }
}
