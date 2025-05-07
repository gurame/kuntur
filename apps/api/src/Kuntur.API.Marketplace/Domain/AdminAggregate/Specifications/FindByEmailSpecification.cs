using Ardalis.Specification;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;

namespace Kuntur.API.Marketplace.Domain.AdminAggregate.Specifications;
internal sealed class FindByUserIdSpecification : Specification<Admin>
{
    public FindByUserIdSpecification(UserId userId)
    {
        Query.Where(user => user.UserId == userId);
    }
}   