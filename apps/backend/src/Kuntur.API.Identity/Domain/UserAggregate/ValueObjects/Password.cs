using System.Text.RegularExpressions;

namespace Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
internal sealed class Password : ValueObject
{
    public string Value { get; }
    private Password(string value)
    {
        Value = value;
    }
    public static Password FromPlainText(string plainText)
    {
        ValidatePasswordPolicy(plainText);
        return new Password(plainText);
    }
    private static void ValidatePasswordPolicy(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters long.");

        if (!Regex.IsMatch(password, @"[A-Z]"))
            throw new ArgumentException("Password must contain at least one uppercase letter.");

        if (!Regex.IsMatch(password, @"[a-z]"))
            throw new ArgumentException("Password must contain at least one lowercase letter.");

        if (!Regex.IsMatch(password, @"[0-9]"))
            throw new ArgumentException("Password must contain at least one number.");

        if (!Regex.IsMatch(password, @"[\W_]"))
            throw new ArgumentException("Password must contain at least one special character.");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}