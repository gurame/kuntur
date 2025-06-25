using RabbitMQ.Client;

namespace Kuntur.Worker.Host.Infrastructure.Messaging.Factories;

public interface IChannelFactory
{
    Task<IChannel> CreateChannelAsync(CancellationToken ct = default);
}