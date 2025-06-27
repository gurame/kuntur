namespace Kuntur.SharedKernel.IntegrationEvents.Marketplace;
public record MarketplaceCreatedIntegrationEvent(Guid MarketplaceId, string Name) : IIntegrationEvent;