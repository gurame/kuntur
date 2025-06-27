using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Kuntur.API.Common.Infrastructure.Messaging;
using Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.Settings;
using Kuntur.SharedKernel.IntegrationEvents;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;

namespace Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.IntegrationEventsPublisher;

internal class IntegrationEventsPublisher : IIntegrationEventsPublisher
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly ILogger<IntegrationEventsPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    public IntegrationEventsPublisher(IOptions<MessageBrokerSettings> messageBrokerOptions,
           ILogger<IntegrationEventsPublisher> logger)
    {
        _messageBrokerSettings = messageBrokerOptions.Value;
        _logger = logger;

        _connectionFactory = new ConnectionFactory
        {
            HostName = _messageBrokerSettings.HostName,
            Port = _messageBrokerSettings.Port,
            UserName = _messageBrokerSettings.UserName,
            Password = _messageBrokerSettings.Password
        };
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        _connection = await _connectionFactory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        await _channel.ExchangeDeclareAsync(_messageBrokerSettings.ExchangeName, ExchangeType.Fanout, durable: true, cancellationToken: ct);
    }

    public async Task PublishEventAsync(IIntegrationEvent @event)
    {
        if (_channel is null)
            throw new InvalidOperationException("Publisher not initialized. Call InitializeAsync() first.");

        string message = JsonSerializer.Serialize(@event);
        byte[] body = Encoding.UTF8.GetBytes(message);

        // [Begin] Diagnostics
        const string operation = "publish";
        string eventType = @event!.GetType().Name;

        var activityName = $"{operation} {eventType}";
        using var activity = RabbitMqDiagnostics.ActivitySource.StartActivity(activityName, ActivityKind.Producer);

        // TODO: How ActivityContext is created and how could be null?
        ActivityContext contextToInject = default;

        if (activity != null)
        {
            contextToInject = activity.Context;
        }
        else if (Activity.Current != null)
        {
            contextToInject = Activity.Current.Context;
        }

        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        _logger.LogInformation("TraceId: {TraceId}", contextToInject.TraceId);

        Baggage.SetBaggage("source_app.name", "kuntur");

        RabbitMqDiagnostics.Propagator.Inject(
            new PropagationContext(contextToInject, Baggage.Current),
            properties,
            InjectTraceContextIntoBasicProperties);

        SetActivityContext(activity, eventType, operation);
        // [End] Diagnostics

        await _channel.BasicPublishAsync(
            exchange: _messageBrokerSettings.ExchangeName,
            mandatory: false,
            routingKey: string.Empty,
            basicProperties: properties,
            body: body);
    }

    private static void SetActivityContext(Activity? activity, string eventType, string operation)
    {
        if (activity is null) return;

        // These tags are added demonstrating the semantic conventions of the OpenTelemetry messaging specification
        // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/messaging/messaging-spans.md
        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.destination_kind", "exchange");
        activity.SetTag("messaging.operation", operation);
        activity.SetTag("messaging.destination.name", eventType);
    }

    private static void InjectTraceContextIntoBasicProperties(IBasicProperties props, string key, string value)
    {
        props.Headers ??= new Dictionary<string, object>()!;
        props.Headers[key] = value;
    }
}