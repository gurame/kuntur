using ErrorOr;
using Kuntur.API.Common.UseCases;
using Kuntur.API.Marketplace.Contracts;
using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.AdminAggregate.Specifications;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;
using Kuntur.API.Marketplace.Interfaces;

namespace Kuntur.API.Marketplace.UseCases.Admins;

internal class CreateAdminProfileCommandHandler(IMarketplaceRepository<Admin> repository) :
    ICommandHandler<CreateAdminCommand, ErrorOr<CreateAdminResponse>>
{
    private readonly IMarketplaceRepository<Admin> _repository = repository;
    public async Task<ErrorOr<CreateAdminResponse>> Handle(CreateAdminCommand request, CancellationToken ct)
    {
        var userId = new UserId(request.UserId);
        if (await _repository.AnyAsync(new FindByUserIdSpecification(userId), ct))
        {
            return DomainErrors.Admin.AlreadyExists;
        }
        
        var admin = new Admin(new AdminId(request.AdminId), userId);
        await _repository.AddAsync(admin, ct);
        await _repository.SaveChangesAsync(ct);

        return new CreateAdminResponse(admin.Id.Value);
    }
}