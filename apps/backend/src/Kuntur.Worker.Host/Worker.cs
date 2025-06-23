using Kuntur.Worker.Host.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Kuntur.Worker.Host;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<Worker> _logger;
    private readonly IConnectionFactory _connectionFactory;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private IConnection? _connection;
    private IChannel? _channel;
    public Worker(IServiceScopeFactory serviceScopeFactory,
                  IOptions<MessageBrokerSettings> messageBrokerOptions,
                  ILogger<Worker> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
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

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _connection = await _connectionFactory.CreateConnectionAsync(ct);

        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

        await _channel.ExchangeDeclareAsync(_messageBrokerSettings.ExchangeName, ExchangeType.Fanout, durable: true, cancellationToken: ct);

        await _channel.QueueDeclareAsync(queue: _messageBrokerSettings.QueueName, durable: false, exclusive: false, autoDelete: false, cancellationToken: ct);

        await _channel.QueueBindAsync(_messageBrokerSettings.QueueName, _messageBrokerSettings.ExchangeName, routingKey: string.Empty, cancellationToken: ct);

        var consumer = new BasicAsyncConsumer(_channel, _serviceScopeFactory, _logger);

        await _channel.BasicConsumeAsync(_messageBrokerSettings.QueueName, autoAck: false, consumer, cancellationToken: ct);

        _logger.LogInformation("Starting integration event consumer background service.");
    }
}
