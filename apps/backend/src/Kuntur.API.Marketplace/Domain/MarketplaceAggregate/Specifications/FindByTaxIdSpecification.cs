using Ardalis.Specification;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate;

namespace Kuntur.API.Marketplace.Domain.MarketplaceAggregate.Specifications;

internal sealed class FindByTaxIdSpecification : Specification<MarketplaceAgg>
{
    public FindByTaxIdSpecification(string taxId)
    {
        Query.Where(marketplace => marketplace.TaxId == taxId);
    }
}