using Kuntur.API.Identity.Domain.Services;
using Kuntur.API.Identity.Infrastructure.Identity;
using Kuntur.API.Identity.Infrastructure.Persistence;
using Kuntur.API.Identity.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kuntur.API.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModuleServices(this IServiceCollection services, 
        IConfiguration configuration,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        string connectionString = configuration.GetConnectionString("IdentityConnection")!;

        services.AddDbContext<KunturIdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IIdentityProvider, KeycloakProvider>();

        services.AddScoped(typeof(IIdentityRepository<>), typeof(IdentityEfRepository<>));

        mediatRAssemblies.Add(typeof(DependencyInjection).Assembly);

        logger.Information("{Module} module services registered", "Identity");

        return services;
    }
}