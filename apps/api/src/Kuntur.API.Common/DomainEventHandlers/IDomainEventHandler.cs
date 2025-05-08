using Kuntur.API.Common.Domain;
using MediatR;

namespace Kuntur.API.Common.DomainEventHandlers;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent> 
    where TEvent : IDomainEvent;