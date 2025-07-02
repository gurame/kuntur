using System.Diagnostics;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Throw;

namespace Kuntur.API.Shared.UseCases.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : IErrorOr
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

            // TODO: Reflection! Could be a performance concern
            // Type myType = request.GetType();
            // IList<PropertyInfo> props = [.. myType.GetProperties()];
            // foreach (PropertyInfo prop in props)
            // {
            //     object? propValue = prop?.GetValue(request, null);
            //     _logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
            // }
        }

        var sw = Stopwatch.StartNew();

        var response = await next(cancellationToken);
        string resultType = nameof(Success);
        LogLevel logLevel = LogLevel.Information;
        if (response.IsError)
        {
            logLevel = LogLevel.Error;
            if (response.Errors is null || response.Errors.Count == 0)
            {
                resultType = "Unknown";
            }
            else if (response.Errors.Count == 1)
            {
                resultType = response.Errors.First().Type.ToString();
            }
            else
            {
                resultType = $"{response.Errors.Count} errors";
            }
        }

        _logger.Log(logLevel, "Handled {RequestName} with {ResultType} in {ms} ms", typeof(TRequest).Name, resultType, sw.ElapsedMilliseconds);
        sw.Stop();

        return response;
    }
}