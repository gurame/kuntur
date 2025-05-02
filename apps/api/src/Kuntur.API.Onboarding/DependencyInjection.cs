using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kuntur.API.Onboarding;
public static class DependencyInjection
{
    public static IServiceCollection AddOnboardingModuleServices(this IServiceCollection services, 
        IConfiguration configuration,
        ILogger logger,
        List<System.Reflection.Assembly> mediatRAssemblies)
    {
        mediatRAssemblies.Add(typeof(DependencyInjection).Assembly);

        logger.Information("{Module} module services registered", "Onboarding");

        return services;
    }
}