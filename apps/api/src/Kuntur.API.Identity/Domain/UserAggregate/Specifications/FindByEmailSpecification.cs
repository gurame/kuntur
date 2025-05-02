using Ardalis.Specification;

namespace Kuntur.API.Identity.Domain.UserAggregate.Specifications;
internal sealed class FindByEmailSpecification : Specification<User>
{
    public FindByEmailSpecification(string email)
    {
        Query.Where(user => user.EmailAddress.Value == email);
    }
}   