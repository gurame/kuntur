using Ardalis.Specification.EntityFrameworkCore;
using Kuntur.API.Shared.Domain;

namespace Kuntur.API.Shared.Infrastructure.Persistence;
public class EfRepository<T>(BaseDbContext dbContext) :
  RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{

}