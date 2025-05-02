using Kuntur.API.Common.UseCases;
using Kuntur.API.Identity.Contracts;

namespace Kuntur.API.Onboarding.UseCases.Marketplaces;
internal record CommandResult(Guid MarketplaceId);
internal record Command(
    string Name, string TaxId,
    string FirstName, string LastName,
    string PhoneNumber, string EmailAddress, string Password) : ICommand<ErrorOr<CommandResult>>
{
    public class Handler(ISender sender) : ICommandHandler<Command, ErrorOr<CommandResult>>
    {
        private readonly ISender _sender = sender;
        public async Task<ErrorOr<CommandResult>> Handle(Command cmd, CancellationToken ct)
        {
            var createUserCommand = new CreateUserCommand(
                cmd.FirstName, cmd.LastName,
                cmd.PhoneNumber, cmd.EmailAddress,
                cmd.Password);

            await _sender.Send(createUserCommand, ct);

            return new CommandResult(Guid.NewGuid());
        }
    }
}