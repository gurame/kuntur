using Kuntur.API.Common.UseCases;
using Kuntur.API.Identity.Contracts;
using Kuntur.API.Marketplace.Contracts;

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
            // Identity
            var createUserCommand = new CreateUserCommand(
                cmd.FirstName, cmd.LastName,
                cmd.EmailAddress, cmd.PhoneNumber,
                cmd.Password);
            var createUserResult = await _sender.Send(createUserCommand, ct);
            if (createUserResult.IsError)
            {
                return createUserResult.Errors;
            }
            var userId = createUserResult.Value.UserId;

            var createAdminProfile = new CreateAdminProfileCommand(userId);
            var createAdminProfileResult = await _sender.Send(createAdminProfile, ct);
            if (createAdminProfileResult.IsError)
            {
                return createAdminProfileResult.Errors;
            }
            var adminId = createAdminProfileResult.Value.AdminId;

            // Marketplace
            var createAdmin = new CreateAdminCommand(userId, adminId);
            var createAdminResult = await _sender.Send(createAdmin, ct);
            if (createAdminResult.IsError)
            {
                return createAdminResult.Errors;
            }

            var createSubscription = new CreateSubscriptionCommand(adminId);
            var createSubscriptionResult = await _sender.Send(createSubscription, ct);
            if (createSubscriptionResult.IsError)
            {
                return createSubscriptionResult.Errors;
            }
            
            return new CreateCommandResult(Guid.NewGuid());
        }
    }
}