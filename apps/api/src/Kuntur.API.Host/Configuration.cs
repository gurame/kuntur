using System.Reflection;
using Carter;
using Kuntur.API.Common;
using Kuntur.API.Host.Documentation;
using Kuntur.API.Host.Logging;
using Kuntur.API.Host.Versioning;

namespace Kuntur.API.Host;
public static class Configuration
{
    public static WebApplicationBuilder AddKunturApiServices(this WebApplicationBuilder builder,
        Serilog.ILogger logger)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCarter();

        builder.AddApiLogging();

        builder.AddApiDocumentation();

        builder.AddApiVersioning();

        logger.Information("{Module} services configured", "Kuntur.API.Host");

        return builder;
    }
    public static WebApplication UseKunturApi(this WebApplication app)
    {
        app.UseApiVersioning();

        app.MapGroup(string.Empty)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .MapCarter();

        app.UseApiDocumentation();

        return app;
    }

    public static WebApplicationBuilder AddKunturModules(this WebApplicationBuilder builder, Serilog.ILogger logger)
    {
        List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];

        builder.Services.AddModules(builder.Configuration, logger, mediatRAssemblies);

        builder.Services.AddMediatR(cfg =>
          cfg.RegisterServicesFromAssemblies([.. mediatRAssemblies]));

        return builder;
    }
}
