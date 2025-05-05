using Kuntur.API.Common;
using Kuntur.API.Common.Infrastructure.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kuntur.API.Onboarding;

public class Configuration : IModuleConfiguration
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        // Register services specific to the module here
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        const string basePath = "onboarding";
        var group = app.MapGroup(basePath);
        group.MapEndopints<Configuration>();
    }
}