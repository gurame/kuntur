using Kuntur.API.Common.Infrastructure.Persistence;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.Infrastructure.Persistence;

internal class IdentityEfRepository<T>(KunturDbContext dbContext) 
    : EfRepository<T>(dbContext), IIdentityRepository<T> where T : class, IAggregateRoot;

