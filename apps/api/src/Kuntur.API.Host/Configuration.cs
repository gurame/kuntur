using Carter;
using Kuntur.API.Common;
using Kuntur.API.Common.Infrastructure;
using Kuntur.API.Common.Infrastructure.Persistence;
using Kuntur.API.Host.Diagnostics;
using Kuntur.API.Host.Documentation;
using Kuntur.API.Host.Logging;
using Kuntur.API.Host.Versioning;

namespace Kuntur.API.Host;
public static class Configuration
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder,
        Serilog.ILogger logger)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCarter();
        builder.Services.AddProblemDetails();

        builder.AddApiLogging();

        builder.AddApiDocumentation();

        builder.AddApiVersioning();

        builder.AddApiDiagnostics();

        builder.AddPersistence();

        logger.Information("{Module} services configured", "Kuntur.API.Host");

        return builder;
    }
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseTransactionalConsistency();
        app.UseStatusCodePages();

        app.UseApiVersioning();

        app.MapGet("/", () => Results.Content(
            """
            <h1>Kuntur API is running</h1>
            <p>Visit <a href="/docs">/docs</a> for documentation.</p>
            """,
            "text/html"
        ))
        .ExcludeFromDescription();

        app.MapGroup(string.Empty)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .MapCarter();

        app.UseApiDocumentation();

        return app;
    }

    public static WebApplicationBuilder AddModules(this WebApplicationBuilder builder, Serilog.ILogger logger)
    {
        builder.Services.AddModules(builder.Configuration, logger);
        return builder;
    }
}
