using ErrorOr;
using Kuntur.API.Common.UseCases;

namespace Kuntur.API.Marketplace.Contracts;
public record ExistsMarketplaceQuery(string TaxtId) : IQuery<ErrorOr<bool>>;