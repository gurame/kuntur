using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Kuntur.Worker.Host.Infrastructure.Messaging.Factories;

public class ChannelFactory : IChannelFactory
{
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private readonly ConnectionFactory _connectionFactory;
    private readonly ILogger<ChannelFactory> _logger;
    public ChannelFactory(IOptions<MessageBrokerSettings> messageBrokerOptions,
                          ILogger<ChannelFactory> logger)
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

    public async Task<IChannel> CreateChannelAsync(CancellationToken ct)
    {
        IConnection? _connection = null;

        const int MAX_RETRIES = 5;
        int retryCount = 0;
        while (_connection == null || !_connection.IsOpen)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new RabbitMQ connection.");
                _connection = await _connectionFactory.CreateConnectionAsync(ct);
            }
            catch (Exception ex)
            {
                retryCount++;
                _logger.LogError(ex, "Failed to create RabbitMQ connection. Attempt {RetryCount} of {MaxRetries}. ", retryCount, MAX_RETRIES);
                if (retryCount >= MAX_RETRIES)
                {
                    throw;
                }
                await Task.Delay(TimeSpan.FromSeconds(2), ct); // Wait before retrying
            }
        }
        return await _connection.CreateChannelAsync(cancellationToken: ct);
    }
}