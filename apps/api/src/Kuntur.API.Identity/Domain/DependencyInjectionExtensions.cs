using Kuntur.API.Identity.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Kuntur.API.Identity.Domain;

internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}