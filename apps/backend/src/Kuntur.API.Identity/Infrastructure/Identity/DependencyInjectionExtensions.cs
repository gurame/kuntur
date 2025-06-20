using Kuntur.API.Common.Infrastructure.Resilience;
using Kuntur.API.Identity.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kuntur.API.Identity.Infrastructure.Identity;
internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddKeycloak(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection("Identity:Keycloak"));

        services.AddHttpClient<IKeycloakTokenService, KeycloakTokenService>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });

        services.AddTransient<BearerTokenHandler>();

        services.AddHttpClient<IIdentityProvider, KeycloakProvider>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddHttpMessageHandler<BearerTokenHandler>()
            .AddHttpResiliencePolicy<KeycloakProvider>();

        return services;
    }
}