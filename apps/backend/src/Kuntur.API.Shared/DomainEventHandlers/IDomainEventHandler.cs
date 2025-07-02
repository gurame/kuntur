using Kuntur.API.Shared.Domain;
using MediatR;

namespace Kuntur.API.Shared.DomainEventHandlers;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent> 
    where TEvent : IDomainEvent;