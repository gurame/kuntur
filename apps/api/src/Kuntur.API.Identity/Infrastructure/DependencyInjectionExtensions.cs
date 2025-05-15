using Kuntur.API.Identity.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuntur.API.Identity.Infrastructure;

internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Identity
        services.AddKeycloak(configuration);
        return services;
    }
}