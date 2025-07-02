namespace Kuntur.API.Shared.Domain;
public abstract class AggregateRoot<TId>(TId id) : Entity<TId>(id), IAggregateRoot
{
    protected readonly List<IDomainEvent> _domainEvents = [];   
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();
        return copy;
    }
}
