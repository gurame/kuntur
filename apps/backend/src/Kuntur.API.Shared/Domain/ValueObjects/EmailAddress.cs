using System.Net.Mail;

namespace Kuntur.API.Shared.Domain.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    public EmailAddress(string value)
    {
        value.ThrowIfEmptyOrWhiteSpace();

        try
        {
            var mailAddress = new MailAddress(value);
            Value = mailAddress.Address.ToLowerInvariant();
        }
        catch
        {
            throw new FormatException("Invalid email address format.");
        }
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        return [Value];
    }
}