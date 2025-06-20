using MediatR;

namespace Kuntur.API.Common.UseCases;
public interface ICommand<out TResponse> : IRequest<TResponse>;