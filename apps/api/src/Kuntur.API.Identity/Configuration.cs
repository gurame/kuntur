using Kuntur.API.Common;
using Kuntur.API.Common.Infrastructure.Endpoints;
using Kuntur.API.Identity.Domain.Services;
using Kuntur.API.Identity.Infrastructure.Identity;
using Kuntur.API.Identity.Infrastructure.Persistence;
using Kuntur.API.Identity.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kuntur.API.Identity;

public class Configuration : IModuleConfiguration
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IIdentityProvider, KeycloakProvider>();

        services.AddScoped(typeof(IIdentityRepository<>), typeof(IdentityEfRepository<>));
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        const string basePath = "identity";
        var group = app.MapGroup(basePath);
        group.MapEndopints<Configuration>();
    }
}
