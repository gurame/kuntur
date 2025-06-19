using System.Diagnostics.Metrics;

namespace Kuntur.API.Onboarding.Infrastructure.Diagnostics;

public static class OnboardingDiagnostics
{
    private const string ServiceName = "Kuntur.API.Onboarding";
    public static readonly Meter Meter = new(ServiceName);
    public static readonly Counter<long> OnboardingSucceededCounter = Meter.CreateCounter<long>("onboarding.succeeded");
}