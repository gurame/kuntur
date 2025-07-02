using Ardalis.Specification;

namespace Kuntur.API.Shared.Domain;
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot;
