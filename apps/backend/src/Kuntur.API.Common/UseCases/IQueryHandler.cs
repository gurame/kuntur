namespace Kuntur.API.Common.UseCases;

using MediatR;
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
       where TQuery : IQuery<TResponse>;