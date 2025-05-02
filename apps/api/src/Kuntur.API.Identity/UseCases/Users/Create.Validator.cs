using FluentValidation;
using Kuntur.API.Identity.Contracts;

namespace Kuntur.API.Identity.UseCases.Users;

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(cmd => cmd.FirstName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("First name must be at most 50 characters long.");

        RuleFor(cmd => cmd.LastName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("Last name must be at most 50 characters long.");

        RuleFor(cmd => cmd.EmailAddress)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email address must be a valid email format.");

        RuleFor(cmd => cmd.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be a valid international format.");
            
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");
    }
}