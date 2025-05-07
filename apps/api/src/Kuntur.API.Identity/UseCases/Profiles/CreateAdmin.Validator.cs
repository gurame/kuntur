
using FluentValidation;
using Kuntur.API.Identity.Contracts;

namespace Kuntur.API.Identity.UseCases.Profiles;
internal class CreateAdminProfileCommandValidator : AbstractValidator<CreateAdminProfileCommand>
{
    public CreateAdminProfileCommandValidator()
    {
        RuleFor(cmd => cmd.UserId)
            .NotNull()
            .WithMessage("UserId must not be null.");
    }
}