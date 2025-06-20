using Throw;

namespace Kuntur.API.Common.Domain.ValueObjects;

public sealed class ContactInfo(Name name, EmailAddress email, PhoneNumber? phone = null) : ValueObject
{
    public Name Name { get; } = name.ThrowIfNull();
    public EmailAddress Email { get; } = email.ThrowIfNull();
    public PhoneNumber Phone { get; } = phone.ThrowIfNull();
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Email;
        yield return Phone;
    }
}
