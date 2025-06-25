using Kuntur.Worker.Host.Infrastructure.Messaging.Factories;
using Kuntur.Worker.Host.Interfaces;
using RabbitMQ.Client;

namespace Kuntur.Worker.Host.Infrastructure.Messaging;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.Section));
        services.AddSingleton<IChannelFactory, ChannelFactory>();
        services.AddSingleton<IAsyncBasicConsumer, RabbitMqBasicAsyncConsumer>();
        services.AddSingleton<IConsumer, RabbitMqConsumer>();

        return services;
    }
}