using System.Reflection;
using Carter;
using Kuntur.API.Host.Configuration.Documentation;
using Kuntur.API.Host.Configuration.Logging;
using Kuntur.API.Host.Configuration.Versioning;
using Kuntur.API.Identity;
using Kuntur.API.Onboarding;
using Log = Serilog;
namespace Kuntur.API.Host.Configuration;
public static class DependencyInjection
{
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder,
        Log.ILogger logger)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCarter();

        builder.AddApiLogging();

        builder.AddApiDocumentation();

        builder.AddApiVersioning();

        logger.Information("API services added");

        return builder;
    }
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseApiVersioning();

        app.MapGroup(string.Empty)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .MapCarter();

        app.UseApiDocumentation();

        return app;
    }

    public static WebApplicationBuilder AddKunturModules(this WebApplicationBuilder builder, Log.ILogger logger)
    {
        List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];

        builder.Services.AddIdentityModuleServices(builder.Configuration, logger, mediatRAssemblies);
        builder.Services.AddOnboardingModuleServices(builder.Configuration, logger, mediatRAssemblies);

        builder.Services.AddMediatR(cfg =>
          cfg.RegisterServicesFromAssemblies([.. mediatRAssemblies]));

        logger.Information("API modules added");

        return builder;
    }
}