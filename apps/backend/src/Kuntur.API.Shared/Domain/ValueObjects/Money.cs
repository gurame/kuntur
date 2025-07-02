
using Throw;

namespace Kuntur.API.Shared.Domain.ValueObjects;

public sealed class Money(decimal amount, CurrencyCode currency) : ValueObject
{
    public decimal Amount { get; } = amount.Throw().IfLessThan(0);
    public CurrencyCode Currency { get; } = currency.ThrowIfNull();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
