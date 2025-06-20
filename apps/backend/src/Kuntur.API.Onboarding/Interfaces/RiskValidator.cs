using Kuntur.API.RiskEvaluator.Host;

namespace Kuntur.API.Onboarding.Interfaces;
internal class RiskValidator(Evaluator.EvaluatorClient evaluatorClient) : IRiskValidator
{
    private readonly Evaluator.EvaluatorClient _evaluatorClient = evaluatorClient;
    public async Task<ErrorOr<bool>> HasAcceptableRiskLevelAsync(string taxId, CancellationToken ct)
    {
        var response = await _evaluatorClient.EvaluateAsync(
            new RiskEvaluationRequest
            {
                TaxId = taxId
            },
            cancellationToken: ct
        );

        return response.RiskLevel != RiskLevel.High;
    }
}