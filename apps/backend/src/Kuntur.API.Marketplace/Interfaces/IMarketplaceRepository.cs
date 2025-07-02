using Kuntur.API.Shared.Domain;

namespace Kuntur.API.Marketplace.Interfaces;
internal interface IMarketplaceRepository<T> : IRepository<T> where T : class, IAggregateRoot;