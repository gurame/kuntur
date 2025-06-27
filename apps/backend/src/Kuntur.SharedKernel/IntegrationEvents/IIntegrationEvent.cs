using System.Text.Json.Serialization;
using Kuntur.SharedKernel.IntegrationEvents.Marketplace;
using MediatR;

namespace Kuntur.SharedKernel.IntegrationEvents;

[JsonDerivedType(typeof(MarketplaceCreatedIntegrationEvent), typeDiscriminator: nameof(MarketplaceCreatedIntegrationEvent))]
public interface IIntegrationEvent : INotification { }