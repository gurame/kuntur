using Kuntur.API.Marketplace.Interfaces;
using Kuntur.API.Shared.Domain;
using Kuntur.API.Shared.Infrastructure.Persistence;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence;

internal class MarketplaceEfRepository<T>(KunturDbContext dbContext)
    : EfRepository<T>(dbContext), IMarketplaceRepository<T> where T : class, IAggregateRoot;