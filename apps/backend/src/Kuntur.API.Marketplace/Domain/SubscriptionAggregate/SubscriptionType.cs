using Ardalis.SmartEnum;

namespace Kuntur.API.Marketplace.Domain.SubscriptionAggregate;

public class SubscriptionType(string name, int value) : SmartEnum<SubscriptionType>(name, value)
{
    public static readonly SubscriptionType Free = new(nameof(Free), 0);
    public static readonly SubscriptionType Starter = new(nameof(Starter), 1);
    public static readonly SubscriptionType Pro = new(nameof(Pro), 2);
    public static readonly SubscriptionType PayAsYouGo = new(nameof(PayAsYouGo), 3);
}