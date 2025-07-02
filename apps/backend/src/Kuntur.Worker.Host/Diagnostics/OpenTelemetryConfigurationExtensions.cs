using System.Reflection;
using Kuntur.API.Shared.Infrastructure.Messaging;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Kuntur.Worker.Host.Diagnostics;
public static class OpenTelemetryConfigurationExtensions
{
    public static HostApplicationBuilder AddOpenTelemetry(this HostApplicationBuilder builder)
    {
        const string serviceName = "Kuntur.Worker";

        var otlpEndpoint = new Uri(builder.Configuration.GetValue<string>("OTLP_Endpoint")!);

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource
                    .AddService(serviceName)
                    .AddAttributes(
                    [
                        new KeyValuePair<string, object>("service.version",
                            Assembly.GetExecutingAssembly().GetName().Version!.ToString())
                    ]);
            })
            .WithTracing(tracing =>
                tracing
                    .AddSource(RabbitMqDiagnostics.ActivitySourceName)
                    .AddOtlpExporter(options =>
                        options.Endpoint = otlpEndpoint)
            )
            .WithMetrics(metrics =>
                metrics
                    .AddOtlpExporter(options =>
                        options.Endpoint = otlpEndpoint)
            )
            .WithLogging(
                logging=>
                    logging
                        .AddOtlpExporter(options => 
                            options.Endpoint = otlpEndpoint)
            );

        return builder;
    }
}