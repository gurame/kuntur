namespace Kuntur.API.Onboarding.Interfaces;

internal interface IRiskValidator
{
    Task<ErrorOr<bool>> HasAcceptableRiskLevelAsync(string taxId, CancellationToken ct);
}