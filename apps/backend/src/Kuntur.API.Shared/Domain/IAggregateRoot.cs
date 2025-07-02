namespace Kuntur.API.Shared.Domain;

public interface IAggregateRoot
{
    void AddDomainEvent(IDomainEvent domainEvent);
    List<IDomainEvent> PopDomainEvents();
}