using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;

namespace Kuntur.API.Identity.Domain.UserAggregate;

internal class User : AggregateRoot<UserId>
{
    private readonly Name _name = default!;
    private readonly PhoneNumber _phoneNumber = default!;
    private AdminId? _adminId = null!;
    public EmailAddress EmailAddress { get; private set; } = default!;
    
    private User() : base(default!) { }
    public User(Name name,
        EmailAddress emailAddress,
        PhoneNumber phoneNumber, UserId? id) : base(id ?? new UserId(Guid.NewGuid()))
    {
        _name = name;
        EmailAddress = emailAddress;
        _phoneNumber = phoneNumber;
    }
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