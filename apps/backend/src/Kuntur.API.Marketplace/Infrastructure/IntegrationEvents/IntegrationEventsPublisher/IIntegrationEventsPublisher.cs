using Kuntur.SharedKernel.IntegrationEvents;

namespace Kuntur.API.Marketplace.Infrastructure.IntegrationEvents.IntegrationEventsPublisher;
internal interface IIntegrationEventsPublisher
{
    Task InitializeAsync(CancellationToken cancellationToken);
    Task PublishEventAsync(IIntegrationEvent integrationEvent);
}