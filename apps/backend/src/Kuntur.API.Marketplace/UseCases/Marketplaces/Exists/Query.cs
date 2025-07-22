using ErrorOr;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate;
using Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate.Specifications;
using Kuntur.API.Marketplace.Interfaces;
using Kuntur.API.Shared.UseCases;

namespace Kuntur.API.Marketplace.UseCases.Marketplaces.Exists;

internal class ExistsMarketplaceQueryHandler(IMarketplaceRepository<TenantMarketplace> repository) :
    IQueryHandler<ExistsMarketplaceQuery, ErrorOr<bool>>
{
    private readonly IMarketplaceRepository<TenantMarketplace> _repository = repository;

    public async Task<ErrorOr<bool>> Handle(ExistsMarketplaceQuery request, CancellationToken ct)
    {
        var specification = new FindByTaxIdSpecification(request.TaxtId);
        return await _repository.AnyAsync(specification, ct);
    }
}