using MediatR;

namespace Kuntur.API.Shared.UseCases;
public interface ICommand<out TResponse> : IRequest<TResponse>;