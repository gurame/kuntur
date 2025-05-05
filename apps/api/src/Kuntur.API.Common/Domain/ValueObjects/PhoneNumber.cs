
using Throw;
using PhoneNumbers;

namespace Kuntur.API.Common.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }
    public CountryCode Country { get; }

    private PhoneNumber() : this(string.Empty, new CountryCode(string.Empty)) {}
    private PhoneNumber(string value, CountryCode country)
    {
        Value = value;
        Country = country;
    }
    public static PhoneNumber Parse(string raw)
    {
        raw.Throw().IfEmpty().IfWhiteSpace();

        var phoneUtil = PhoneNumberUtil.GetInstance();
        var parsed = phoneUtil.Parse(raw, null);

        if (!phoneUtil.IsValidNumber(parsed))
            throw new FormatException("Invalid phone number");

        var countryCode = new CountryCode(phoneUtil.GetRegionCodeForNumber(parsed));

        return new PhoneNumber(raw, countryCode);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}