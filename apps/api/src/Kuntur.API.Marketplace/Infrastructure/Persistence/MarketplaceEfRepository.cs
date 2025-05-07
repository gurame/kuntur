using Kuntur.API.Common.Domain;
using Kuntur.API.Common.Infrastructure.Persistence;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence;

internal class MarketplaceEfRepository<T>(KunturMarketplaceDbContext dbContext) 
    : EfRepository<T>(dbContext), IMarketplaceRepository<T> where T : class, IAggregateRoot;