using ErrorOr;

namespace Kuntur.API.Shared.Domain.EventualConsistency;

public class EventualConsistencyException(Error eventualConsistencyError, List<Error>? underlyingErrors = null)
    : Exception(eventualConsistencyError.Description)
{
    public Error EventualConsistencyError { get; } = eventualConsistencyError;
    public List<Error> UnderlyingErrors { get; } = underlyingErrors ?? [];
}