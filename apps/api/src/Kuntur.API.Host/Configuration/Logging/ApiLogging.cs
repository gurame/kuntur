using Serilog;

namespace Kuntur.API.Host.Configuration.Logging;

public static class ApiDocumentation
{
    public static WebApplicationBuilder AddApiLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, config) =>
            config.ReadFrom.Configuration(builder.Configuration));

        return builder;
    }
}