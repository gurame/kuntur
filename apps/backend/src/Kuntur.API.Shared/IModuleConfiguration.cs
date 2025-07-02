using System.Reflection;
using Carter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuntur.API.Shared;
public interface IModuleConfiguration : ICarterModule
{
    public static abstract void AddServices(IServiceCollection services,
        IConfiguration configuration,
        Serilog.ILogger logger);
}