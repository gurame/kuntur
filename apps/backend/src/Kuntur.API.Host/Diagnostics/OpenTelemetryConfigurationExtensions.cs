using System.Reflection;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Kuntur.API.Shared.Infrastructure.Messaging;
using Kuntur.API.Onboarding.Diagnostics;
using Kuntur.API.Shared.Diagnostics;

namespace Kuntur.API.Host.Diagnostics;

public static class OpenTelemetryConfigurationExtensions
{
    public static WebApplicationBuilder AddApiDiagnostics(this WebApplicationBuilder builder)
    {
        const string serviceName = "Kuntur.API";

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
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddNpgsql()
                    .AddSource(RabbitMqDiagnostics.ActivitySourceName)
                    .AddApiInstrumentation()
                    .AddOtlpExporter(options =>
                        options.Endpoint = otlpEndpoint)
                )
            .WithMetrics(metrics =>
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOnboardingInstrumentation()
                    .AddOtlpExporter(options =>
                        options.Endpoint = otlpEndpoint)
            )
            .WithLogging(
                logging =>
                    logging
                        .AddOtlpExporter(options =>
                            options.Endpoint = otlpEndpoint)
            );
        // TODO: LogLevel

        return builder;
    }
}