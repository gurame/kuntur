using Kuntur.API.Shared.Infrastructure.Consistency;
using Microsoft.AspNetCore.Builder;

namespace Kuntur.API.Shared.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseTransactionalConsistency(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}