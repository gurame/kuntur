using ErrorOr;

namespace Kuntur.API.Identity.Domain.UserAggregate;

public static class DomainErrors
{
    public static class Admin
    {
        public static readonly Error AlreadyExists
            = Error.Conflict(code: "Admin.AlreadyExists", description: "Admin with this user id already exists");
    }
}