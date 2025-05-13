using Kuntur.API.Identity.Domain.UserAggregate;
using Kuntur.API.Identity.Domain.UserAggregate.Specifications;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Kuntur.API.Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Kuntur.API.Identity.Domain.Services;

internal interface IUserService
{
    Task<ErrorOr<UserId>> CreateUserAsync(
        string firstName,
        string lastName,
        string rawEmailAddress,
        string rawPhoneNumber,
        string rawPassword,
        CancellationToken ct);

    Task<ErrorOr<AdminId>> MapRoleAsync(
        UserId userId,
        Roles roles,
        CancellationToken ct);
}

internal class UserService(IIdentityProvider identityProvider,
    IIdentityRepository<User> repository,
    ILogger<UserService> logger) : IUserService
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly IIdentityRepository<User> _repository = repository;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<ErrorOr<UserId>> CreateUserAsync(
        string firstName, string lastName,
        string rawEmailAddress, string rawPhoneNumber,
        string rawPassword,
        CancellationToken ct)
    {
        var emailAddress = new EmailAddress(rawEmailAddress);
        if (await _repository.AnyAsync(new FindByEmailSpecification(emailAddress), ct))
            return DomainErrors.User.ExistingEmail;

        var password = Password.FromPlainText(rawPassword);
        var identityResult = await _identityProvider.CreateUserAsync(emailAddress, password, ct);
        if (identityResult.IsError)
            return identityResult.Errors;

        var user = new User(
            name: new Name(firstName, lastName),
            emailAddress: emailAddress,
            phoneNumber: PhoneNumber.Parse(rawPhoneNumber),
            id: identityResult.Value);

        try
        {
            await _repository.AddAsync(user, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {UserId} in database", user.Id);
            await _identityProvider.DeleteUserAsync(identityResult.Value, ct);
            return DomainErrors.Persistence.SaveChanges;
        }

        return user.Id;
    }

    public async Task<ErrorOr<AdminId>> MapRoleAsync(
        UserId userId,
        Roles roles,
        CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return DomainErrors.User.NotFound;
        }

        var result = user.CreateAdminProfile();
        if (result.IsError)
        {
            return result.Errors;
        }
        var adminId = result.Value;

        await _repository.UpdateAsync(user, ct);

        var identityResult = await _identityProvider.MapRoleToUserAsync(userId, roles, ct);
        if (identityResult.IsError)
        {
            return identityResult.Errors;
        }

        return adminId;
    }
}