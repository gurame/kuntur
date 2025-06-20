using Ardalis.Specification.EntityFrameworkCore;
using Kuntur.API.Common.Domain;

namespace Kuntur.API.Common.Infrastructure.Persistence;
public class EfRepository<T>(BaseDbContext dbContext) :
  RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{

}