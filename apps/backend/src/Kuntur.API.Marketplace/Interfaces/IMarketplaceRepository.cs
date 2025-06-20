using Kuntur.API.Common.Domain;

namespace Kuntur.API.Marketplace.Interfaces;
internal interface IMarketplaceRepository<T> : IRepository<T> where T : class, IAggregateRoot;