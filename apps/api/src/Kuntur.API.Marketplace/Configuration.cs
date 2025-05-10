using Kuntur.API.Common;
using Kuntur.API.Common.Infrastructure.Endpoints;
using Kuntur.API.Marketplace.Infrastructure.Persistence;
using Kuntur.API.Marketplace.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kuntur.API.Marketplace;

public class Configuration : IModuleConfiguration
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services.AddScoped(typeof(IMarketplaceRepository<>), typeof(MarketplaceEfRepository<>));
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        const string basePath = "marketplace";
        var group = app.MapGroup(basePath);
        group.MapEndopints<Configuration>();
    }
}
