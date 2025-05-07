using Kuntur.API.Common.Domain;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate;

internal class Admin : AggregateRoot<AdminId>
{
    public UserId UserId { get; }
    public Guid SubscriptionId { get; private set; }

    private Admin() : base(default!) { }
    public Admin(AdminId adminId, UserId userId) : base(adminId)
    {
        UserId = userId;
    }
}