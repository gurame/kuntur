using Throw;

namespace Kuntur.API.Common.Domain.Extensions;
public static class ThrowExtensions
{
    public static string ThrowIfEmptyOrWhiteSpace(this string value)
    {
        value.Throw().IfEmpty().IfWhiteSpace();
        return value!;
    }
}