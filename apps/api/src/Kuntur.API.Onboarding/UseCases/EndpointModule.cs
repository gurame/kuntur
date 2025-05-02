
using Kuntur.API.Common.Infrastructure.Endpoints;

namespace Kuntur.API.Onboarding.UseCases;
public class OnboardingModule : ICarterModule
{
    private const string basePath = "onboarding";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(basePath);
        group.MapEndopints<OnboardingModule>();
    }
}