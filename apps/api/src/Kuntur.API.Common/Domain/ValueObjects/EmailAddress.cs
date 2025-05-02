
using Throw;

namespace Kuntur.API.Common.Domain.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        value.ThrowIfEmptyOrWhiteSpace();

        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(value);
            Value = mailAddress.Address.ToLowerInvariant();
        }
        catch
        {
            throw new FormatException("Invalid email address format.");
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents() => [Value];
}
