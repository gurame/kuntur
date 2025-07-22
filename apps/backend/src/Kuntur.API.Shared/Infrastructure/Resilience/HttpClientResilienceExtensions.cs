using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Kuntur.API.Shared.Infrastructure.Resilience;

public static class HttpClientResilienceExtensions
{
    public static IHttpClientBuilder AddHttpResiliencePolicy<T>(this IHttpClientBuilder builder)
    {
        return builder.AddPolicyHandler((services, request) =>
        {
            var logger = services.GetRequiredService<ILogger<T>>();

            // Apply retry policy only to idempotent requests
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(
                    5,
                    retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                        TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)),
                    (outcome, timespan, retryAttempt, context) =>
                    {
                        logger.LogWarning(outcome.Exception, "Retry {RetryAttempt} after {Delay} due to {Reason}",
                            retryAttempt, timespan, outcome.Exception?.Message ?? outcome.Result?.ReasonPhrase);
                    });

            // Circuit breaker for any request
            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    5,
                    TimeSpan.FromSeconds(30),
                    (outcome, breakDelay) =>
                    {
                        logger.LogWarning("Circuit broken due to {Reason}, retrying after {BreakDelay}",
                            outcome.Exception?.Message ?? outcome.Result?.ReasonPhrase, breakDelay);
                    },
                    () => logger.LogInformation("Circuit reset"));

            return request.Method == HttpMethod.Get || request.Method == HttpMethod.Head ||
                   request.Method == HttpMethod.Put || request.Method == HttpMethod.Delete
                ? Policy.WrapAsync(retryPolicy, circuitBreakerPolicy)
                : circuitBreakerPolicy;
        });
    }
}