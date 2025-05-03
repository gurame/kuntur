using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace Kuntur.API.Common.Infrastructure.Endpoints;
public static class EndpointExtensions
{
    public static void MapEndopints<TMarker>(this RouteGroupBuilder app)
    {
        MapEndopints(app, typeof(TMarker));
    }

    public static void MapEndopints(this RouteGroupBuilder app, Type typeMarker)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);

        foreach (var endpointType in endpointTypes)
        {
            try
            {
                endpointType.GetMethod(nameof(IEndpoint.Define))!
                .Invoke(null, [app]);

                Log.Information("{Module} endpoints configured", endpointType.Assembly.GetName().Name);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure {Module} endpoints", endpointType.Assembly.GetName().Name);
                continue;
            }
        }
    }

    private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining(Type typeMarker)
    {
        var endpointTypes = typeMarker.Assembly.DefinedTypes
            .Where(x => !x.IsAbstract && !x.IsInterface &&
                        typeof(IEndpoint).IsAssignableFrom(x));
        return endpointTypes;
    }
}
