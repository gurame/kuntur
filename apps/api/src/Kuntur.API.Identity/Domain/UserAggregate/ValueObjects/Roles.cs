namespace Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

internal sealed class Roles : ValueObject
{
    private readonly HashSet<RoleItem> _roles;
    public IReadOnlyCollection<RoleItem> Values => _roles;
    private Roles(IEnumerable<RoleItem> roles)
    {
        _roles = [.. roles];
    }
    public static Roles From(params string[] roleNames)
    {
        if (roleNames is null || roleNames.Length == 0)
            throw new ArgumentException("At least one role must be specified.");

        var invalid = roleNames
            .Where(name => !RoleConstants.IsValidName(name))
            .ToList();

        if (invalid.Any())
            throw new ArgumentException($"Invalid roles: {string.Join(", ", invalid)}");

        var validRoles = roleNames
            .Select(RoleConstants.GetByName)!
            .Where(r => r is not null)!
            .ToList();

        return new Roles(validRoles!);
    }

    public bool Contains(string roleName) =>
        _roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));

    public bool ContainsId(Guid roleId) =>
        _roles.Any(r => r.Id == roleId);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        foreach (var role in _roles.OrderBy(r => r.Id))
        {
            yield return role.Id;
            yield return role.Name.ToLowerInvariant();
        }
    }
    public override string ToString() =>
        string.Join(", ", _roles.Select(r => r.ToString()));
}
internal sealed class RoleItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }

    public override string ToString() => $"{Name} ({Id})";
}
internal static class RoleConstants
{
    public static readonly RoleItem MarketplaceAdmin = new() { Id = Guid.Parse("597bea85-5b44-4fcb-8ee7-ad3207d43a58"), Name = "marketplace-admin" };
    public static readonly RoleItem SellerAdmin = new() { Id = Guid.Parse("01201993-4b2f-4ea7-9412-f739ca27c2ca"), Name = "seller-admin" };
    public static readonly RoleItem Buyer = new() { Id = Guid.Parse("941ff82c-c958-4007-b5f5-7893fe189664"), Name = "buyer" };

    public static readonly List<RoleItem> All = [MarketplaceAdmin, SellerAdmin, Buyer];

    public static bool IsValidName(string name) =>
        All.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public static RoleItem? GetByName(string name) =>
        All.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}