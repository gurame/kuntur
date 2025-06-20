using Ardalis.Specification;

namespace Kuntur.API.Common.Domain;
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot;
    