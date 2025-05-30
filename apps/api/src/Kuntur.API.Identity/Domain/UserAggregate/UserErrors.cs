namespace Kuntur.API.Identity.Domain.UserAggregate;

public static class DomainErrors
{
    public static class Persistence
    {
        public static readonly Error SaveChanges
            = Error.Failure(code: "Persistence.Failure", description: "Could not save changes to the database");
    }

    public static class User
    {
        public static readonly Error NotFound
            = Error.NotFound(code: "User.NotFound", description: "User not found");
        public static readonly Error AlreadyHasAdminProfile
            = Error.Conflict(code: "User.AlreadyHasAdminProfile", description: "User already has an admin profile");
        public static readonly Error ExistingEmail
            = Error.Conflict(code: "User.ExistingEmail", description: "User with this email already exists");
    }
}