using Grpc.Core;

namespace Kuntur.API.RiskEvaluator.Host.Services;

public class EvaluatorService(ILogger<EvaluatorService> logger) : Evaluator.EvaluatorBase
{
    private readonly ILogger<EvaluatorService> _logger = logger;
    public override Task<RiskEvaluationReply> Evaluate(RiskEvaluationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Evaluating risk for request: {@Request}", request);
        return Task.FromResult(new RiskEvaluationReply
        {
            RiskLevel = RiskLevel.Low
        });
    }
}
