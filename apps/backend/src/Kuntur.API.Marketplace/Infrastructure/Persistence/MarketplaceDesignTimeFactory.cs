using Kuntur.API.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence;

public class MarketplaceDesignTimeFactory : BaseDesignTimeFactory,
    IDesignTimeDbContextFactory<MarketplaceDbContext>
{
    public MarketplaceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseNpgsql(ConnectionString,
            options => { options.MigrationsHistoryTable(MigrationTableName, "marketplace"); });

        return new MarketplaceDbContext(optionsBuilder.Options);
    }
}