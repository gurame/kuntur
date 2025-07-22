using ErrorOr;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate;

public static class AdminErrors
{
    public static readonly Error AlreadyExists
        = Error.Conflict("Admin.AlreadyExists", "Admin with this user id already exists");

    public static readonly Error AlreadyHasSubscriptionSet
        = Error.Conflict("Admin.AlreadyHasSubscriptionSet", "Admin already has a subscription set");

    public static readonly Error NotFound
        = Error.NotFound("Admin.NotFound", "Admin not found");
}