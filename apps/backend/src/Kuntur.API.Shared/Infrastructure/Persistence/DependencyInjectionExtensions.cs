using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuntur.API.Shared.Infrastructure.Persistence;
public static class DependencyInjectionExtensions
{
    public static void AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<KunturDbContext>(options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });
    }
}