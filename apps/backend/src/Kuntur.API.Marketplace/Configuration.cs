using System.Data;
using Kuntur.API.Shared;
using Kuntur.API.Shared.Infrastructure.Endpoints;
using Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.BackgroundService;
using Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.IntegrationEventsPublisher;
using Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.Settings;
using Kuntur.API.Marketplace.Infrastructure.Persistence;
using Kuntur.API.Marketplace.Infrastructure.Persistence.Outbox;
using Kuntur.API.Marketplace.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Serilog;

namespace Kuntur.API.Marketplace;

public class Configuration : IModuleConfiguration
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        // Add Options
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.Section));

        // Add Repositories
        services.AddScoped(typeof(IMarketplaceRepository<>), typeof(MarketplaceEfRepository<>));
        // Outbox
        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        // Add Background Services
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<PublishIntegrationEventsBackgroundService>();
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        const string basePath = "marketplace";
        var group = app.MapGroup(basePath);
        group.MapEndopints<Configuration>();
    }
}
