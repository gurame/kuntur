using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Serilog;

namespace Kuntur.API.Common;

public static class ModuleConfigurationExtensions
{
    public static void AddModules(this IServiceCollection services, IConfiguration configuration,
        ILogger logger, List<Assembly> mediatRAssemblies)
    {
        var moduleConfigTypes = GetModuleConfigurationTypesFromAllAssemblies(logger);

        foreach (var moduleConfigType in moduleConfigTypes)
        {
            try
            {
                moduleConfigType.GetMethod(nameof(IModuleConfiguration.AddServices))!
                .Invoke(null, [services, configuration, logger, mediatRAssemblies]);

                Log.Information("{Module} services configured", moduleConfigType.Assembly.GetName().Name);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure {Module} services", moduleConfigType.Assembly.GetName().Name);
                continue;
            }
        }
    }

    public static IEnumerable<TypeInfo> GetModuleConfigurationTypesFromAllAssemblies(ILogger logger)
    {
        var entryAssembly = Assembly.GetEntryAssembly()!;
        var assemblies = new List<Assembly> { entryAssembly };
        var deps = DependencyContext.Load(entryAssembly)!;
        var projectLibs = deps.RuntimeLibraries.Where(lib => lib.Type == "project");

        foreach (var lib in projectLibs)
        {
            foreach (var name in lib.GetDefaultAssemblyNames(deps))
            {
                try
                {
                    assemblies.Add(Assembly.Load(name));
                }
                catch
                {
                    logger.Warning("Failed to load assembly {AssemblyName}", name);
                }
            }
        }

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