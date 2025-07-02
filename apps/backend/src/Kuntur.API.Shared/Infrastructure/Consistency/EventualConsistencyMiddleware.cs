using Kuntur.API.Shared.Domain;
using Kuntur.API.Shared.Domain.EventualConsistency;
using Kuntur.API.Shared.Infrastructure.Persistence;
using MediatR;

using Microsoft.AspNetCore.Http;

namespace Kuntur.API.Shared.Infrastructure.Consistency;
public class EventualConsistencyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, KunturDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () =>
        {
            bool success = context.Response.StatusCode < 400;
            try
            {
                if (success)
                {
                    if (context.Items.TryGetValue(Constants.DomainEventsKey, out var value) &&
                        value is Queue<IDomainEvent> domainEvents)
                    {
                        while (domainEvents.TryDequeue(out var nextEvent))
                        {
                            await publisher.Publish(nextEvent);
                        }
                    }

                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            catch (EventualConsistencyException)
            {
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });
        await _next(context);
    }
}
