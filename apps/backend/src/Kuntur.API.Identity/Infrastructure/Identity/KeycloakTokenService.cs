using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Kuntur.API.Identity.Infrastructure.Identity;

public interface IKeycloakTokenService
{
    Task<string> GetTokenAsync();
}
public class KeycloakTokenService(HttpClient httpClient, IOptions<KeycloakOptions> options) : IKeycloakTokenService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly KeycloakOptions _options = options.Value;
    private string? _cachedToken;
    private DateTime _expiresAt;

    public async Task<string> GetTokenAsync()
    {
        // TODO: Seems _cachedToken is null always, check if the token is cached correctly
        if (_cachedToken is not null && DateTime.UtcNow < _expiresAt)
            return _cachedToken;

        var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["grant_type"] = "client_credentials"
            })
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        _cachedToken = json.GetProperty("access_token").GetString();
        var expiresIn = json.GetProperty("expires_in").GetInt32();
        _expiresAt = DateTime.UtcNow.AddSeconds(expiresIn - 60);

        return _cachedToken!;
    }
}