namespace Kuntur.API.Common.Infrastructure.IntegrationEvents.Marketplace;
public record MarketplaceCreatedIntegrationEvent(Guid MarketplaceId, string Name) : IIntegrationEvent;