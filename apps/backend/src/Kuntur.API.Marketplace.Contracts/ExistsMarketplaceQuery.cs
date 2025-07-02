using ErrorOr;
using Kuntur.API.Shared.UseCases;

namespace Kuntur.API.Marketplace.Contracts;
public record ExistsMarketplaceQuery(string TaxtId) : IQuery<ErrorOr<bool>>;