using System.Diagnostics;
using OpenTelemetry.Context.Propagation;

namespace Kuntur.API.Common.Infrastructure.Messaging;

public static class RabbitMqDiagnostics
{
    public static readonly string ActivitySourceName = "RabbitMq";
    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
    public static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
}