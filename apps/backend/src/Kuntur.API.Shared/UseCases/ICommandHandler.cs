using MediatR;

namespace Kuntur.API.Shared.UseCases;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>;