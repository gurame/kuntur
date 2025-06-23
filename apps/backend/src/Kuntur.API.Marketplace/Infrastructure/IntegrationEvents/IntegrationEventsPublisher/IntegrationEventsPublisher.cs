using System.Text;
using System.Text.Json;
using Kuntur.API.Common.Infrastructure.IntegrationEvents;
using Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.IntegrationEventsPublisher;

internal class IntegrationEventsPublisher : IIntegrationEventsPublisher
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private IConnection? _connection;
    private IChannel? _channel;
    public IntegrationEventsPublisher(IOptions<MessageBrokerSettings> messageBrokerOptions,
           ILogger <IntegrationEventsPublisher> logger)
    {
        _messageBrokerSettings = messageBrokerOptions.Value;
        logger.LogInformation("HostName: {HostName}, Port: {Port}, UserName: {UserName}, ExchangeName: {ExchangeName}",
            _messageBrokerSettings.HostName, _messageBrokerSettings.Port, _messageBrokerSettings.UserName, _messageBrokerSettings.ExchangeName);
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

    public async Task PublishEventAsync(IIntegrationEvent integrationEvent)
    {
        if (_channel is null)
            throw new InvalidOperationException("Publisher not initialized. Call InitializeAsync() first.");

        string serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent);

        byte[] body = Encoding.UTF8.GetBytes(serializedIntegrationEvent);

        await _channel.BasicPublishAsync(
            exchange: _messageBrokerSettings.ExchangeName,
            routingKey: string.Empty,
            body: body);
    }
}