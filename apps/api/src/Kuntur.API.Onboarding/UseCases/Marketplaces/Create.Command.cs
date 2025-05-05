using Kuntur.API.Common.UseCases;
using Kuntur.API.Identity.Contracts;

namespace Kuntur.API.Onboarding.UseCases.Marketplaces;
internal record CreateCommandResult(Guid MarketplaceId);
internal record CreateCommand(
    string Name, string TaxId,
    string FirstName, string LastName,
    string EmailAddress, string PhoneNumber, string Password) : ICommand<ErrorOr<CreateCommandResult>>
{
    public class Handler(ISender sender) : ICommandHandler<CreateCommand, ErrorOr<CreateCommandResult>>
    {
        private readonly ISender _sender = sender;
        public async Task<ErrorOr<CreateCommandResult>> Handle(CreateCommand cmd, CancellationToken ct)
        {
            var createUserCommand = new CreateUserCommand(
                cmd.FirstName, cmd.LastName,
                cmd.EmailAddress, cmd.PhoneNumber,
                cmd.Password);

            var createUserResult = await _sender.Send(createUserCommand, ct);
            if (createUserResult.IsError)
            {
                return createUserResult.Errors;
            }
            
            return new CreateCommandResult(Guid.NewGuid());
        }
    }
}