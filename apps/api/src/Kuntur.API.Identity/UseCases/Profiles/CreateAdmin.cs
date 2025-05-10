using Kuntur.API.Identity.Contracts;
using Kuntur.API.Identity.Domain.UserAggregate;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.UseCases.Profiles;

internal class CreateAdminProfileCommandHandler(IIdentityRepository<User> repository) :
    ICommandHandler<CreateAdminProfileCommand, ErrorOr<CreateAdminProfileResponse>>
{
    private readonly IIdentityRepository<User> _repository = repository;
    public async Task<ErrorOr<CreateAdminProfileResponse>> Handle(CreateAdminProfileCommand request, CancellationToken ct)
    {
        var userId = new UserId(request.UserId);
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

        return new CreateAdminProfileResponse(adminId.Value);
    }
}