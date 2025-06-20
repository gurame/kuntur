using MediatR;

namespace Kuntur.API.Common.UseCases;
public interface IQuery<out TResponse> : IRequest<TResponse>;