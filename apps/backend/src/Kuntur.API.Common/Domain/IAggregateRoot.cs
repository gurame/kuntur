namespace Kuntur.API.Common.Domain;

public interface IAggregateRoot
{
    void AddDomainEvent(IDomainEvent domainEvent);
    List<IDomainEvent> PopDomainEvents();
}