namespace Kuntur.API.Identity.Domain.UserAggregate;

public static class DomainErrors
{
    public static class Persistence
    {
        public static readonly Error SaveChanges
            = Error.Failure("Persistence.Failure", "Could not save changes to the database");
    }

    public static class User
    {
        public static readonly Error NotFound
            = Error.NotFound("User.NotFound", "User not found");

        public static readonly Error AlreadyHasAdminProfile
            = Error.Conflict("User.AlreadyHasAdminProfile", "User already has an admin profile");

        public static readonly Error ExistingEmail
            = Error.Conflict("User.ExistingEmail", "User with this email already exists");
    }
}