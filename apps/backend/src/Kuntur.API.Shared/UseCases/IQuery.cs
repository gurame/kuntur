using MediatR;

namespace Kuntur.API.Shared.UseCases;
public interface IQuery<out TResponse> : IRequest<TResponse>;