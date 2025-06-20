using Microsoft.Extensions.Configuration;

namespace Kuntur.API.Common.Infrastructure.Persistence;

public abstract class BaseDesignTimeFactory
{
    protected static string MigrationTableName => "__EFMigrationsHistory";
    protected string ConnectionString { get; private set; }
    public BaseDesignTimeFactory()
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
}