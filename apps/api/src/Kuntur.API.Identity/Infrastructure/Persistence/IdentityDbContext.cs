using Kuntur.API.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kuntur.API.Identity.Infrastructure.Persistence;
public class IdentityDbContext(DbContextOptions options) : 
    KunturDbContext(options, null!, [typeof(IdentityDbContext).Assembly])
{
    
}