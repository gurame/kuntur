using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using Serilog;

namespace Kuntur.API.Common;

public static class AssemblyHelper
{
    private static readonly Lazy<IEnumerable<Assembly>> _projectsAssemblies = new(() =>
    {
        var logger = Log.Logger;
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

        return assemblies;
    });
    public static IEnumerable<Assembly> ProjectsAssemblies => _projectsAssemblies.Value;
}