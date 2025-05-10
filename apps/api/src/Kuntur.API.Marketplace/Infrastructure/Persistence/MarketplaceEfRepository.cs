using Kuntur.API.Common.Domain;
using Kuntur.API.Common.Infrastructure.Persistence;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence;

internal class MarketplaceEfRepository<T>(KunturDbContext dbContext) 
    : EfRepository<T>(dbContext), IMarketplaceRepository<T> where T : class, IAggregateRoot;