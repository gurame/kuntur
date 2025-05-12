using System.Reflection;
using FluentValidation;
using Kuntur.API.Common.UseCases.Behaviors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Serilog;

namespace Kuntur.API.Common;

public static class ModuleConfigurationExtensions
{
    public static void AddModules(this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {

        // Specific module services configuration
        var moduleConfigTypes = GetModuleConfigurationTypesFromAllAssemblies();
        foreach (var moduleConfigType in moduleConfigTypes)
        {
            try
            {
                moduleConfigType.GetMethod(nameof(IModuleConfiguration.AddServices))!
                .Invoke(null, [services, configuration, logger]);

                logger.Information("{Module} services configured", moduleConfigType.Assembly.GetName().Name);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to configure {Module} services", moduleConfigType.Assembly.GetName().Name);
                continue;
            }
        }

        // Cross-module services configuration
        var assemblies = moduleConfigTypes.Select(x => x.Assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies([.. assemblies]);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            // cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            cfg.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
    }

    private static List<TypeInfo> GetModuleConfigurationTypesFromAllAssemblies()
    {
        var assemblies = AssemblyHelper.ProjectsAssemblies;

        List<TypeInfo> moduleConfigurationTypes = [];
        foreach (var assembly in assemblies)
        {
            var types = assembly.DefinedTypes
            .Where(x => !x.IsAbstract && !x.IsInterface &&
                        typeof(IModuleConfiguration).IsAssignableFrom(x));

            moduleConfigurationTypes.AddRange(types);
        }

        return moduleConfigurationTypes;
    }

}