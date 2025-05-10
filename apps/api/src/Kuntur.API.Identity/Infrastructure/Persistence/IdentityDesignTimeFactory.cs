using Kuntur.API.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kuntur.API.Identity.Infrastructure.Persistence;
public class IdentityDesignTimeFactory : BaseDesignTimeFactory, IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseNpgsql(ConnectionString, options =>
        {
            options.MigrationsHistoryTable(MigrationTableName, "identity");
        });

        return new IdentityDbContext(optionsBuilder.Options);
    }
}