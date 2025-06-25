using Kuntur.Worker.Host.Infrastructure.Messaging.Factories;
using Kuntur.Worker.Host.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Kuntur.Worker.Host.Infrastructure.Messaging;

public class RabbitMqConsumer(IChannelFactory channelFactory,
    IAsyncBasicConsumer asyncBasicConsumer,
    IOptions<MessageBrokerSettings> messageBrokerOptions) : IConsumer
{
    private readonly IChannelFactory _channelFactory = channelFactory;
    private readonly IAsyncBasicConsumer _asyncBasicConsumer = asyncBasicConsumer;
    private readonly MessageBrokerSettings _messageBrokerSettings = messageBrokerOptions.Value;
    public async Task ConsumeAsync(CancellationToken ct)
    {
        var channel = await _channelFactory.CreateChannelAsync(ct);

        // [Begin] Declare the exchange and queue, and bind them (DevOps should ensure these are created in production)
        await channel.ExchangeDeclareAsync(_messageBrokerSettings.ExchangeName, ExchangeType.Fanout, durable: true, cancellationToken: ct);
        await channel.QueueDeclareAsync(queue: _messageBrokerSettings.QueueName, durable: false, exclusive: false, autoDelete: false, cancellationToken: ct);
        await channel.QueueBindAsync(_messageBrokerSettings.QueueName, _messageBrokerSettings.ExchangeName, routingKey: string.Empty, cancellationToken: ct);
        // [End] Declare the exchange and queue, and bind them

        // Assign the channel to the consumer if it implements RabbitMqBasicAsyncConsumer
        if (_asyncBasicConsumer is RabbitMqBasicAsyncConsumer consumerWithChannel)
        {
            consumerWithChannel.Channel = channel;
        }

        await channel.BasicConsumeAsync(_messageBrokerSettings.QueueName, autoAck: false, _asyncBasicConsumer, cancellationToken: ct);
    }
}