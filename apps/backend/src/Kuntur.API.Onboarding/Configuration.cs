using Kuntur.API.Shared;
using Kuntur.API.Shared.Infrastructure.Endpoints;
using Kuntur.API.Onboarding.Interfaces;
using Kuntur.API.RiskEvaluator.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kuntur.API.Onboarding;

public class Configuration : IModuleConfiguration
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services.AddScoped<IRiskValidator, RiskValidator>();
        services.AddGrpcClient<Evaluator.EvaluatorClient>(options =>
        {
            options.Address = new Uri(configuration["RiskEvaluator:Url"]!);
        });
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        const string basePath = "onboarding";
        var group = app.MapGroup(basePath);
        group.MapEndopints<Configuration>();
    }
}