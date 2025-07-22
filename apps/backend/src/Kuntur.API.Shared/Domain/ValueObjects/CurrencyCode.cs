using Throw;

namespace Kuntur.API.Shared.Domain.ValueObjects;

public sealed class CurrencyCode : ValueObject
{
    public CurrencyCode(string value)
    {
        value.Throw().IfNotMatches(@"^[A-Z]{3}$");
        Value = value.ToUpperInvariant();
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}