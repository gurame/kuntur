using Kuntur.Worker.Host.Infrastructure.Messaging;

namespace Kuntur.Worker.Host.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessaging(configuration);
        return services;
    }
}