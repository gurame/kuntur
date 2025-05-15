using Kuntur.API.Identity.Domain.OrganizationAggregate.ValueObjects;

namespace Kuntur.API.Identity.Domain.OrganizationAggregate;
internal class Organization : AggregateRoot<OrganizationId>
{
    private Organization() : base(default!) { }
    public Organization(OrganizationId? id) : base(id ?? new OrganizationId(Guid.NewGuid()))
    {
 
    }
}