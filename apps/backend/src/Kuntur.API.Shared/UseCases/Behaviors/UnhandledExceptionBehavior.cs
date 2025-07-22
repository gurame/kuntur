using System.Diagnostics;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kuntur.API.Shared.UseCases.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>(
    ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            Activity.Current?.SetStatus(ActivityStatusCode.Error, ex.Message);
            Activity.Current?.AddException(ex);

            var requestName = request.GetType().Name;

            _logger.LogError(ex, "An unhandled exception occurred while processing request {Request}", requestName);

            return (dynamic)Error.Unexpected(
                "UnhandledException",
                $"An unhandled exception occurred while processing the request {requestName}",
                new Dictionary<string, object>
                {
                    { "RequestName", requestName },
                    { "ExceptionMessage", ex.Message },
                    { "StackTrace", ex.StackTrace! }
                }
            );
        }
    }
}