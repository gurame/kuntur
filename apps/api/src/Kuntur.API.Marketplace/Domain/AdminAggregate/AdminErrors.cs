using ErrorOr;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate;

public static class AdminErrors
{
    public static readonly Error AlreadyExists
        = Error.Conflict(code: "Admin.AlreadyExists", description: "Admin with this user id already exists");
    public static readonly Error AlreadyHasSubscriptionSet
        = Error.Conflict(code: "Admin.AlreadyHasSubscriptionSet", description: "Admin already has a subscription set");
    public static readonly Error NotFound
        = Error.NotFound(code: "Admin.NotFound", description: "Admin not found");
}