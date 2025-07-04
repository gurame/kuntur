using ErrorOr;
using Kuntur.API.Shared.UseCases;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.AdminAggregate.Specifications;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.UseCases.Admins.Create;

internal class CreateAdminProfileCommandHandler(IMarketplaceRepository<Admin> repository) :
    ICommandHandler<CreateAdminCommand, ErrorOr<CreateAdminResponse>>
{
    private readonly IMarketplaceRepository<Admin> _repository = repository;
    public async Task<ErrorOr<CreateAdminResponse>> Handle(CreateAdminCommand request, CancellationToken ct)
    {
        var userId = new UserId(request.UserId);
        if (await _repository.AnyAsync(new FindByUserIdSpecification(userId), ct))
        {
            return AdminErrors.AlreadyExists;
        }
        
        var admin = new Admin(userId);
        await _repository.AddAsync(admin, ct);

        return new CreateAdminResponse(admin.Id.Value);
    }
}