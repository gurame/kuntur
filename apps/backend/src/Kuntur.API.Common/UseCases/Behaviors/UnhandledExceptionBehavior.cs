using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kuntur.API.Common.UseCases.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger = logger;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            string requestName = request.GetType().Name;

            _logger.LogError(ex, "An unhandled exception occurred while processing request {Request}", requestName);

            return (dynamic)Error.Unexpected(
                code: "UnhandledException",
                description: $"An unhandled exception occurred while processing the request {requestName}",
                metadata: new Dictionary<string, object>
                {
                    { "RequestName", requestName },
                    { "ExceptionMessage", ex.Message },
                    { "StackTrace", ex.StackTrace! },
                }
            );
        }
    }
}