using Ardalis.Specification;

namespace Kuntur.API.Identity.Domain.UserAggregate.Specifications;
internal sealed class FindByEmailSpecification : Specification<User>
{
    public FindByEmailSpecification(EmailAddress emailAddress)
    {
        Query.Where(user => user.EmailAddress == emailAddress);
    }
}   