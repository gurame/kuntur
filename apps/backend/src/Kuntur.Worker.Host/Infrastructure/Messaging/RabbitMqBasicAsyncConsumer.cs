using System.Text;
using System.Text.Json;
using Kuntur.API.Common.Infrastructure.IntegrationEvents;
using MediatR;
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
}