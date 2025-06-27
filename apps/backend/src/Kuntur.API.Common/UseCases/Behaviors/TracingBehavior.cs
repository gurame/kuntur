using ErrorOr;
using Kuntur.API.Common.Diagnostics;
using MediatR;

namespace Kuntur.API.Common.UseCases.Behaviors;

public class TracingBehavior<TRequest, TResponse>() : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var requestName = requestType.Name;
        var assemblyName = requestType.Assembly.GetName().Name?.Replace(".Contracts", "");
        var moduleName = assemblyName?.Split('.').LastOrDefault();
        var activityName = $"{moduleName} {requestName}";

        using var activity = ApplicationDiagnostics.ActivitySource
            .StartActivity(activityName);

        var response = await next(cancellationToken);

        return response;
    }
}