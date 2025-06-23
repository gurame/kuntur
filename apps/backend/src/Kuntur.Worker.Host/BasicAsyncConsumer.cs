using System.Text;
using System.Text.Json;
using Kuntur.API.Common.Infrastructure.IntegrationEvents;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Throw;

namespace Kuntur.Worker.Host;

public class BasicAsyncConsumer(
    IChannel channel,
    IServiceScopeFactory serviceScopeFactory,
    ILogger logger) : IAsyncBasicConsumer
{
    public IChannel Channel => channel;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger _logger = logger;
    public async Task HandleBasicDeliverAsync(
        string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
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
            _logger.LogInformation("Received integration event. Reading message from queue.");

            using var scope = _serviceScopeFactory.CreateScope();

            var message = Encoding.UTF8.GetString(body.Span);

            var integrationEvent = JsonSerializer.Deserialize<IIntegrationEvent>(message);
            integrationEvent.ThrowIfNull();

            _logger.LogInformation(
                "Received integration event of type: {IntegrationEventType}. Publishing event.",
                integrationEvent.GetType().Name);

            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            await publisher.Publish(integrationEvent, cancellationToken);

            _logger.LogInformation("Integration event published successfully. Sending ack to message broker.");

            await channel.BasicAckAsync(deliveryTag, multiple: false, cancellationToken: cancellationToken);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred while consuming integration event");
        }

    }

    public Task HandleChannelShutdownAsync(object channel, ShutdownEventArgs reason) => Task.CompletedTask;
    public Task HandleBasicCancelOkAsync(string consumerTag, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task HandleBasicCancelAsync(string consumerTag, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task HandleBasicConsumeOkAsync(string consumerTag, CancellationToken cancellationToken = default) => Task.CompletedTask;

}
