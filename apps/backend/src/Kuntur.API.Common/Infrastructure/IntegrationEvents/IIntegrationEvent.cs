using System.Text.Json.Serialization;
using Kuntur.API.Common.Infrastructure.IntegrationEvents.Marketplace;
using MediatR;

namespace Kuntur.API.Common.Infrastructure.IntegrationEvents;

[JsonDerivedType(typeof(MarketplaceCreatedIntegrationEvent), typeDiscriminator: nameof(MarketplaceCreatedIntegrationEvent))]
public interface IIntegrationEvent : INotification { }