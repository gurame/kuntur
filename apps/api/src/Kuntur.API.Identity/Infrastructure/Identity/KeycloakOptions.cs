namespace Kuntur.API.Identity.Infrastructure.Identity;
public class KeycloakOptions
{
    public string BaseUrl { get; set; } = default!;
    public string Realm { get; set; } = "kuntur"; 
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public string TokenEndpoint => $"/realms/{Realm}/protocol/openid-connect/token";
}