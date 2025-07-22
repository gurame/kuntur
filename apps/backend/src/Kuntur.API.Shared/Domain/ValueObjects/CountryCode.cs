using Throw;

namespace Kuntur.API.Shared.Domain.ValueObjects;

public sealed class CountryCode : ValueObject
{
    public CountryCode(string value)
    {
        value.Throw().IfNotMatches(@"^[A-Z]{2}$");
        Value = value.ToUpperInvariant();
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}