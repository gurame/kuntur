using ErrorOr;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate;

public static class DomainErrors
{
    public static class Admin
    {
        public static readonly Error AlreadyExists
            = Error.Conflict(code: "Admin.AlreadyExists", description: "Admin with this user id already exists");

        public static readonly Error NotFound
            = Error.NotFound(code: "Admin.NotFound", description: "Admin not found");
    }
}