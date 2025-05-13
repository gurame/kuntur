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
            //## Identity module

            // Create user
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

            // Create admin profile
            var createAdminProfile = new CreateAdminProfileCommand(userId);
            
            var createAdminProfileResult = await _sender.Send(createAdminProfile, ct);
            if (createAdminProfileResult.IsError)
            {
                return createAdminProfileResult.Errors;
            }
            var adminId = createAdminProfileResult.Value.AdminId;

            //## Marketplace module

            // Create admin
            var createAdmin = new CreateAdminCommand(userId, adminId);
            var createAdminResult = await _sender.Send(createAdmin, ct);
            if (createAdminResult.IsError)
            {
                return createAdminResult.Errors;
            }

            // Create subscription
            var createSubscription = new CreateSubscriptionCommand(adminId);
            var createSubscriptionResult = await _sender.Send(createSubscription, ct);
            if (createSubscriptionResult.IsError)
            {
                return createSubscriptionResult.Errors;
            }
            var subscriptionId = createSubscriptionResult.Value.SubscriptionId;
            
            // Create marketplace
            var createMarketplace = new CreateMarketplaceCommand(subscriptionId, cmd.TaxId, cmd.Name);
            var createMarketplaceResult = await _sender.Send(createMarketplace, ct);
            if (createMarketplaceResult.IsError)
            {
                return createMarketplaceResult.Errors;
            }
            
            return new CreateCommandResult(createMarketplaceResult.Value.MarketplaceId);
        }
    }
}