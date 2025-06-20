using ErrorOr;
using Kuntur.API.Common.UseCases;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate.Specifications;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.UseCases.Marketplaces.Exists;

internal class ExistsMarketplaceQueryHandler(IMarketplaceRepository<MarketplaceAgg> repository) :
    IQueryHandler<ExistsMarketplaceQuery, ErrorOr<bool>>
{
    private readonly IMarketplaceRepository<MarketplaceAgg> _repository = repository;
    public async Task<ErrorOr<bool>> Handle(ExistsMarketplaceQuery request, CancellationToken ct)
    {
        var specification = new FindByTaxIdSpecification(request.TaxtId);
        return await _repository.AnyAsync(specification, ct);
    }
}