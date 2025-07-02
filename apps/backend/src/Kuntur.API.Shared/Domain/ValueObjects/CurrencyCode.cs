using Throw;

namespace Kuntur.API.Shared.Domain.ValueObjects;

public sealed class CurrencyCode : ValueObject
{
    public string Value { get; }
    public CurrencyCode(string value)
    {
        value.Throw().IfNotMatches(@"^[A-Z]{3}$");
        Value = value.ToUpperInvariant();
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
