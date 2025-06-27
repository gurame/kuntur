using OpenTelemetry.Metrics;

namespace Kuntur.API.Onboarding.Diagnostics;

public static class OpenTelemetryConfigurationExtensions
{
    public static MeterProviderBuilder AddOnboardingInstrumentation(this MeterProviderBuilder builder)
    {
        builder.AddMeter(ApplicationDiagnostics.OnboardingSucceededCounter.Meter.Name);

        return builder;
    }
}