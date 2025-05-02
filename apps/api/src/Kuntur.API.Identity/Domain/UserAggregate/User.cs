using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

namespace Kuntur.API.Identity.Domain.UserAggregate;

internal class User(
    Name name,
    EmailAddress emailAddress,
    PhoneNumber phoneNumber, UserId? id) : AggregateRoot<UserId>(id ?? new UserId(Guid.NewGuid()))
{
    private readonly Name _name = name;
    private readonly PhoneNumber _phoneNumber = phoneNumber;
    private AdminId? _adminId = null;
    public EmailAddress EmailAddress { get; private set; } = emailAddress;
    public ErrorOr<AdminId> CreateAdminProfile()
    {
        if (_adminId is not null)
        {
            return DomainErrors.User.AlreadyHasAdminProfile;
        }

        _adminId = new AdminId(Guid.NewGuid());

        return _adminId.Value;
    }
}