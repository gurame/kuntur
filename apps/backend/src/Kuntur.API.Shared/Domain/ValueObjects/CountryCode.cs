using Throw;

namespace Kuntur.API.Shared.Domain.ValueObjects;

public sealed class CountryCode : ValueObject
{
    public string Value { get; }
    public CountryCode(string value)
    {
        value.Throw().IfNotMatches(@"^[A-Z]{2}$");
        Value = value.ToUpperInvariant();
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
