using Ardalis.Specification;

namespace Kuntur.API.Common.Domain;
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot;
