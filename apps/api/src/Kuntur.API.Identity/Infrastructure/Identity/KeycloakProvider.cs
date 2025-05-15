using System.Net.Http.Json;
using Kuntur.API.Identity.Domain.OrganizationAggregate.ValueObjects;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.Infrastructure.Identity;

internal class KeycloakProvider(HttpClient httpClient) : IIdentityProvider
{
    private readonly string realm = "kuntur";
    private readonly HttpClient _httpClient = httpClient;
    public async Task<ErrorOr<UserId>> CreateUserAsync(
        Name name,
        EmailAddress emailAddress,
        Password password,
        CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync($"/admin/realms/{realm}/users", new
        {
            username = emailAddress.Value,
            email = emailAddress.Value,
            firstName = name.FirstName,
            lastName = name.LastName,
            enabled = true,
            emailVerified = true,
            credentials = new[]
            {
                new
                {
                    type = "password",
                    value = password.Value,
                    temporary = false
                }
            }
        }, cancellationToken: ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return Error.Failure("IdentityProvider.CreateUser", error);
        }

        var user = await _httpClient.GetFromJsonAsync<KeyclockUserRepresentation>(response.Headers.Location, ct);
        if (user is null)
        {
            return Error.Failure("IdentityProvider.CreateUser", "Failed to deserialize user response");
        }

        return new UserId(user.Id);
    }
    public async Task<ErrorOr<Success>> DeleteUserAsync(UserId userId, CancellationToken ct)
    {
        var response = await _httpClient.DeleteAsync($"/admin/realms/{realm}/users/{userId.Value}", ct);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return Error.Failure("IdentityProvider.DeleteUser", error);
        }

        return Result.Success;
    }

    public async Task<ErrorOr<OrganizationId>> CreateOrganizationAsync(string name, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync($"/admin/realms/{realm}/organizations", new
        {
            name = name,
            domains = new string[] { name },
        }, cancellationToken: ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return Error.Failure("IdentityProvider.CreateOrganization", error);
        }

        var organization = await _httpClient.GetFromJsonAsync<KeycloakOrganizationRepresentation>(response.Headers.Location, ct);
        if (organization is null)
        {
            return Error.Failure("IdentityProvider.CreateOrganization", "Failed to deserialize organization response");
        }

        return new OrganizationId(organization.Id);
    }

    public async Task<ErrorOr<Success>> AddMemberToOrganizationAsync(OrganizationId organizationId, UserId userId, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync($"/admin/realms/{realm}/organizations/{organizationId.Value}/members", new
        {
            userId = userId.Value,
            membershipType = "admin"
        }, ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return Error.Failure("IdentityProvider.MapUserToOrganization", error);
        }

        return Result.Success;
    }
}
