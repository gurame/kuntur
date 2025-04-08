namespace Kuntur.Backend.Common;

public abstract class AggregateRoot<TId>(TId id) : Entity<TId>(id)
{
    private readonly List<DomainEvent> _domainEvents = [];   
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents() => _domainEvents.Clear();
}
