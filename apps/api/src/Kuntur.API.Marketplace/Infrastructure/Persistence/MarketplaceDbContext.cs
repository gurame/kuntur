using Kuntur.API.Common.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence;

internal sealed class KunturMarketplaceDbContext(
    DbContextOptions<KunturMarketplaceDbContext> options,
    IHttpContextAccessor httpContextAccessor) : BaseDbContext(options, httpContextAccessor)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Marketplace");

        builder.ApplyConfigurationsFromAssembly(typeof(KunturMarketplaceDbContext).Assembly);

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return base.SaveChangesAsync(ct);
    }
}