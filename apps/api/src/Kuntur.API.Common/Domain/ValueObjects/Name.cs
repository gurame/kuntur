namespace Kuntur.API.Common.Domain.ValueObjects;
public sealed class Name(string firstName, string lastName) : ValueObject
{
    public string FirstName { get; } = firstName.ThrowIfEmptyOrWhiteSpace();
    public string LastName { get; } = lastName.ThrowIfEmptyOrWhiteSpace();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName.ToLowerInvariant();
        yield return LastName.ToLowerInvariant();
    }
}