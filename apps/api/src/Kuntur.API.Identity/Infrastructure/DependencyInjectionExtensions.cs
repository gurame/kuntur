using Kuntur.API.Identity.Infrastructure.Identity;
using Kuntur.API.Identity.Infrastructure.Persistence;
using Kuntur.API.Identity.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuntur.API.Identity.Infrastructure;

internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Identity
        services.AddKeycloak(configuration);

        // Persistence
        services.AddScoped(typeof(IIdentityRepository<>), typeof(IdentityEfRepository<>));

        return services;
    }
}