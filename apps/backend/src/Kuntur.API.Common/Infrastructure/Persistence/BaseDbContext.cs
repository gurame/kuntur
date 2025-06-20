using Kuntur.API.Common.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kuntur.API.Common.Infrastructure.Persistence;
public abstract class BaseDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null) // This is only in design time
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        var domainEvents = ChangeTracker.Entries<IAggregateRoot>()
            .Select(x => x.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        Queue<IDomainEvent> domainEventsQueue =
            httpContext.Items.TryGetValue(Constants.DomainEventsKey, out var value) && value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        httpContext.Items[Constants.DomainEventsKey] = domainEventsQueue;

        return result;
    }
}