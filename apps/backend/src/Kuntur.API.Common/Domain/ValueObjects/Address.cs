using Throw;

namespace Kuntur.API.Common.Domain.ValueObjects;

public sealed class Address(string street, string city, string state, string zipCode, CountryCode country) : ValueObject
{
    public string Street { get; } = street.ThrowIfEmptyOrWhiteSpace();
    public string City { get; } = city.ThrowIfEmptyOrWhiteSpace();
    public string State { get; } = state.ThrowIfEmptyOrWhiteSpace();
    public string ZipCode { get; } = zipCode.ThrowIfEmptyOrWhiteSpace();
    public CountryCode Country { get; } = country.ThrowIfNull();
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
        yield return Country;
    }
}
