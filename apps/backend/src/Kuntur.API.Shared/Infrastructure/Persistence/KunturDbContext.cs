using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Kuntur.API.Shared.Infrastructure.Persistence;

public class KunturDbContext : BaseDbContext
{
    private Assembly[] _assemblies = default!;
    public KunturDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
    : base(options, httpContextAccessor)
    {
    }
    public KunturDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor,
        Assembly[] assemblies)
        : base(options, httpContextAccessor)
    {
        _assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _assemblies ??= [.. DesignFactoryHelper.GetTypesFromAllAssemblies().Select(x => x.Assembly)];
        foreach (var assembly in _assemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}