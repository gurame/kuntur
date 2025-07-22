using Ardalis.Specification;

namespace Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate.Specifications;

internal sealed class FindByTaxIdSpecification : Specification<TenantMarketplace>
{
    public FindByTaxIdSpecification(string taxId)
    {
        Query.Where(marketplace => marketplace.TaxId == taxId);
    }
}