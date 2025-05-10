using Kuntur.API.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence;
public class MarketplaceDbContext(DbContextOptions options) : 
    KunturDbContext(options, null!, [typeof(MarketplaceDbContext).Assembly])
{
    
}