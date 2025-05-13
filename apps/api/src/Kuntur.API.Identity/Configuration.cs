using Kuntur.API.Common;
using Kuntur.API.Common.Infrastructure.Endpoints;
using Kuntur.API.Identity.Domain;
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
        services.AddDomain();
        services.AddInfrastructure(configuration);
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        const string basePath = "identity";
        var group = app.MapGroup(basePath);
        group.MapEndopints<Configuration>();
    }
}
