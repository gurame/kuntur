using System.Net.Http.Headers;

namespace Kuntur.API.Identity.Infrastructure.Identity;
public class BearerTokenHandler(IKeycloakTokenService tokenService) : DelegatingHandler
{
    private readonly IKeycloakTokenService _tokenService = tokenService;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}