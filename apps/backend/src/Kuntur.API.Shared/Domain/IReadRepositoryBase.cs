using Ardalis.Specification;

namespace Kuntur.API.Shared.Domain;
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot;
    