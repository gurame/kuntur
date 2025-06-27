using System.Diagnostics.Metrics;

namespace Kuntur.API.Onboarding.Diagnostics;

public static class ApplicationDiagnostics
{
    private const string ServiceName = "Kuntur.API.Onboarding";
    public static readonly Meter Meter = new(ServiceName);
    public static readonly Counter<long> OnboardingSucceededCounter = Meter.CreateCounter<long>("onboarding.succeeded");
}