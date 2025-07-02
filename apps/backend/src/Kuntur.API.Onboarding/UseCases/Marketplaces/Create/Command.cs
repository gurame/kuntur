using Kuntur.API.Shared.UseCases;
using Kuntur.API.Identity.Contracts;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Onboarding.Diagnostics;
using Kuntur.API.Onboarding.Interfaces;
using Microsoft.Extensions.Logging;

namespace Kuntur.API.Onboarding.UseCases.Marketplaces.Create;

internal record CreateCommandResult(Guid MarketplaceId);
internal record CreateCommand(
    string Name, string TaxId,
    string FirstName, string LastName,
    string EmailAddress, string PhoneNumber, string Password) : ICommand<ErrorOr<CreateCommandResult>>
{
    public class Handler(ISender sender,
                         IRiskValidator riskValidator,
                         ILogger<CreateCommand> logger) : ICommandHandler<CreateCommand, ErrorOr<CreateCommandResult>>
    {
        private readonly ISender _sender = sender;
        private readonly IRiskValidator _riskValidator = riskValidator;
        private readonly ILogger<CreateCommand> _logger = logger;
        public async Task<ErrorOr<CreateCommandResult>> Handle(CreateCommand cmd, CancellationToken ct)
        {
            // [MARKETPLACE] Validate existing marketplace
            var existsMarketplaceQuery = new ExistsMarketplaceQuery(cmd.TaxId);
            var existsMarketplaceResult = await _sender.Send(existsMarketplaceQuery, ct);
            if (existsMarketplaceResult.IsError)
            {
                return existsMarketplaceResult.Errors;
            }

            if (existsMarketplaceResult.Value)
            {
                return Error.Conflict("Marketplace with the same tax ID already exists.");
            }

            // [RISK_EVALUATOR] Validate risk
            var riskValidationResult = await _riskValidator.HasAcceptableRiskLevelAsync(cmd.TaxId, ct);
            if (riskValidationResult.IsError)
            {
                return riskValidationResult.Errors;
            }

            if (!riskValidationResult.Value)
            {
                _logger.LogWarning("Risk validation failed for tax ID: {TaxId}", cmd.TaxId);
                return Error.Validation("Risk validation failed. Please contact support.");
            }

            // [IDENTITY] Create user
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

            // [MARKETPLACE] Create admin
            var createAdmin = new CreateAdminCommand(userId);
            var createAdminResult = await _sender.Send(createAdmin, ct);
            if (createAdminResult.IsError)
            {
                return createAdminResult.Errors;
            }
            var adminId = createAdminResult.Value.AdminId;

            // [MARKETPLACE] Create subscription
            var createSubscription = new CreateSubscriptionCommand(adminId);
            var createSubscriptionResult = await _sender.Send(createSubscription, ct);
            if (createSubscriptionResult.IsError)
            {
                return createSubscriptionResult.Errors;
            }
            var subscriptionId = createSubscriptionResult.Value.SubscriptionId;

            // [IDENTITY] Create organization
            var createOrganization = new CreateOrganizationCommand(cmd.Name);
            var createOrganizationResult = await _sender.Send(createOrganization, ct);
            if (createOrganizationResult.IsError)
            {
                return createOrganizationResult.Errors;
            }
            var organizationId = createOrganizationResult.Value.OrganizationId;

            // [IDENTITY] Add member to organization
            var addMemberToOrganization = new AddMemberToOrganizationCommand(organizationId, userId);
            var addMemberToOrganizationResult = await _sender.Send(addMemberToOrganization, ct);
            if (addMemberToOrganizationResult.IsError)
            {
                return addMemberToOrganizationResult.Errors;
            }

            // [MARKETPLACE] Create marketplace
            var createMarketplace = new CreateMarketplaceCommand(organizationId, subscriptionId, cmd.TaxId, cmd.Name);
            var createMarketplaceResult = await _sender.Send(createMarketplace, ct);
            if (createMarketplaceResult.IsError)
            {
                return createMarketplaceResult.Errors;
            }

            ApplicationDiagnostics.OnboardingSucceededCounter.Add(1,
                new KeyValuePair<string, object?>("marketplaceId", createMarketplaceResult.Value.MarketplaceId.ToString()));

            return new CreateCommandResult(createMarketplaceResult.Value.MarketplaceId);
        }
    }
}