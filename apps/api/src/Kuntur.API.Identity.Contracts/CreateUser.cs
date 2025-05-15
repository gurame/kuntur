using ErrorOr;
using Kuntur.API.Common.UseCases;

namespace Kuntur.API.Identity.Contracts;

public record CreateUserResponse(Guid UserId);
public record CreateUserCommand(string FirstName,
    string LastName,
    string EmailAddress,
    string PhoneNumber,
    string Password) : ICommand<ErrorOr<CreateUserResponse>>;

