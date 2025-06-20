using Kuntur.API.Common.Infrastructure.Consistency;
using Microsoft.AspNetCore.Builder;

namespace Kuntur.API.Common.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseTransactionalConsistency(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}