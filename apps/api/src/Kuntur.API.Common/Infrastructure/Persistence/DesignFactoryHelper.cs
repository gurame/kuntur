using System.Reflection;

namespace Kuntur.API.Common.Infrastructure.Persistence;

public static class DesignFactoryHelper
{
    public static IEnumerable<TypeInfo> GetTypesFromAllAssemblies()
    {
        var assemblies = AssemblyHelper.ProjectsAssemblies;

        List<TypeInfo> modelConfigurationTypes = [];
        foreach (var assembly in assemblies)
        {
            var types = assembly.DefinedTypes
            .Where(x => !x.IsAbstract && !x.IsInterface &&
                        typeof(BaseDesignTimeFactory).IsAssignableFrom(x));

            modelConfigurationTypes.AddRange(types);
        }

        return modelConfigurationTypes;
    }
}