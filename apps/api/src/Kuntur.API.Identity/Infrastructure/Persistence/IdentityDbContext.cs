using Kuntur.API.Common.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kuntur.API.Identity.Infrastructure.Persistence;

internal sealed class KunturIdentityDbContext(
    DbContextOptions<KunturIdentityDbContext> options,
    IHttpContextAccessor httpContextAccessor) : BaseDbContext(options, httpContextAccessor)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Identity");

        builder.ApplyConfigurationsFromAssembly(typeof(KunturIdentityDbContext).Assembly);

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return base.SaveChangesAsync(ct);
    }
}