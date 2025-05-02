namespace Kuntur.API.Identity.Interfaces;
internal interface IIdentityRepository<T> : IRepository<T> where T : class, IAggregateRoot;