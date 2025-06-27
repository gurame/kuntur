using OpenTelemetry.Trace;

namespace Kuntur.API.Common.Diagnostics;

public static class OpenTelemetryConfigurationExtensions
{
    public static TracerProviderBuilder AddApiInstrumentation(this TracerProviderBuilder builder)
    {
        builder.AddSource(ApplicationDiagnostics.ActivitySource.Name);
        return builder;
    }
}