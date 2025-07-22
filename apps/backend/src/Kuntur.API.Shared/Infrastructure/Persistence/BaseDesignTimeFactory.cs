using Microsoft.Extensions.Configuration;

namespace Kuntur.API.Shared.Infrastructure.Persistence;

public abstract class BaseDesignTimeFactory
{
    public BaseDesignTimeFactory()
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();

        ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    protected static string MigrationTableName => "__EFMigrationsHistory";
    protected string ConnectionString { get; private set; }
}