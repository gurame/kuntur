using System.Diagnostics;

namespace Kuntur.API.Common.Diagnostics;

public static class ApplicationDiagnostics
{
    public const string ActivitySourceName = "Kuntur.API";
    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}