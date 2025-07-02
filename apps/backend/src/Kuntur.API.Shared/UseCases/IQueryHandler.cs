namespace Kuntur.API.Shared.UseCases;

using MediatR;
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
       where TQuery : IQuery<TResponse>;