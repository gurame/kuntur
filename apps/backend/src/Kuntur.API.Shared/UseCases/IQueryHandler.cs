using MediatR;

namespace Kuntur.API.Shared.UseCases;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;