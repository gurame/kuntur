using OpenTelemetry.Trace;

namespace Kuntur.API.Shared.Diagnostics;

public static class OpenTelemetryConfigurationExtensions
{
    public static TracerProviderBuilder AddApiInstrumentation(this TracerProviderBuilder builder)
    {
        builder.AddSource(ApplicationDiagnostics.ActivitySource.Name);
        return builder;
    }
}