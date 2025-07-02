using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Kuntur.API.Shared.Infrastructure.Messaging;
using Kuntur.SharedKernel.IntegrationEvents;
using MediatR;
using OpenTelemetry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Throw;

namespace Kuntur.Worker.Host.Infrastructure.Messaging;

public class RabbitMqBasicAsyncConsumer(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<RabbitMqBasicAsyncConsumer> logger) : IAsyncBasicConsumer
{
    public IChannel? Channel { get; set; }

    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger _logger = logger;

    public async Task HandleBasicDeliverAsync(
        string consumerTag,
        ulong deliveryTag,
        bool redelivered,
        string exchange,
        string routingKey,
        IReadOnlyBasicProperties properties,
        ReadOnlyMemory<byte> body,
        CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Cancellation requested, not consuming integration event.");
            return;
        }

        try
        {
            _logger.LogInformation("Received integration event from exchange '{Exchange}' with routing key '{RoutingKey}'.", exchange, routingKey);

            foreach (var header in properties.Headers ?? Enumerable.Empty<KeyValuePair<string, object?>>())
            {
                _logger.LogInformation("Header: {Key} = {Value}", header.Key, Encoding.UTF8.GetString((byte[])header.Value!));
            }

            var parentContext = RabbitMqDiagnostics.Propagator.Extract(default,
                properties,
                ExtractTraceContextFromBasicProperties);
            Baggage.Current = parentContext.Baggage;

            // Start an activity with a name following the semantic convention of the OpenTelemetry messaging specification.
            // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/messaging/messaging-spans.md
            const string operation = "process";
            var activityName = $"{operation} {routingKey}";

            using var activity = RabbitMqDiagnostics.ActivitySource.StartActivity(activityName, ActivityKind.Consumer, parentContext.ActivityContext);
            SetActivityContext(activity, routingKey, operation);
            activity?.SetTag("source_app.name", Baggage.Current.GetBaggage("source_app.name"));

            using var scope = _serviceScopeFactory.CreateScope();
            var message = Encoding.UTF8.GetString(body.Span);
            var integrationEvent = JsonSerializer.Deserialize<IIntegrationEvent>(message);
            integrationEvent.ThrowIfNull();

            _logger.LogInformation("Publishing integration event of type: {IntegrationEventType}", integrationEvent.GetType().Name);

            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            await publisher.Publish(integrationEvent, cancellationToken);

            if (Channel is null)
            {
                _logger.LogError("Channel was not set in consumer. Cannot send ack.");
                return;
            }

            await Channel.BasicAckAsync(deliveryTag, multiple: false, cancellationToken: cancellationToken);

            _logger.LogInformation("Integration event acknowledged successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process integration event.");
        }
    }

    public Task HandleChannelShutdownAsync(object channel, ShutdownEventArgs reason)
    {
        _logger.LogWarning("Channel shutdown: {Reason}", reason.ReplyText);
        return Task.CompletedTask;
    }

    public Task HandleBasicCancelOkAsync(string consumerTag, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consumer cancellation acknowledged: {ConsumerTag}", consumerTag);
        return Task.CompletedTask;
    }

    public Task HandleBasicCancelAsync(string consumerTag, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Consumer was cancelled unexpectedly: {ConsumerTag}", consumerTag);
        return Task.CompletedTask;
    }

    public Task HandleBasicConsumeOkAsync(string consumerTag, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consumer registration successful: {ConsumerTag}", consumerTag);
        return Task.CompletedTask;
    }

    private static void SetActivityContext(Activity? activity, string eventName, string operation)
    {
        if (activity is null) return;
        // These tags are added demonstrating the semantic conventions of the OpenTelemetry messaging specification
        // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/messaging/messaging-spans.md
        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.destination_kind", "queue");
        activity.SetTag("messaging.operation", operation);
        activity.SetTag("messaging.destination.name", eventName);
    }
    private static IEnumerable<string> ExtractTraceContextFromBasicProperties(IReadOnlyBasicProperties props, string key)
    {
        if (!props.Headers!.TryGetValue(key, out var value)) return [];

        var bytes = value as byte[];
        return [Encoding.UTF8.GetString(bytes!)];
    }
}