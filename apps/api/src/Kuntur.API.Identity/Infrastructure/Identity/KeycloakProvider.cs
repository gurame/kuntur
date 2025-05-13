using System.Net.Http.Json;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Kuntur.API.Identity.Interfaces;

namespace Kuntur.API.Identity.Infrastructure.Identity;

internal class KeycloakProvider(HttpClient httpClient) : IIdentityProvider
{
    private readonly string realm = "kuntur";
    private readonly HttpClient _httpClient = httpClient;
    public async Task<ErrorOr<UserId>> CreateUserAsync(
        EmailAddress emailAddress,
        Password password,
        CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync($"/admin/realms/{realm}/users", new
        {
            username = emailAddress.Value,
            email = emailAddress.Value,
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

    public async Task<ErrorOr<Success>> MapRoleToUserAsync(UserId userId, Roles roles, CancellationToken ct)
    {
        var payload = roles.Values.Select(role => new
        {
            id = role.Id,
            name = role.Name
        }).ToList();

        var response = await _httpClient.PostAsJsonAsync($"/admin/realms/{realm}/users/{userId.Value}/role-mappings/realm", 
            payload, cancellationToken: ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            return Error.Failure("IdentityProvider.MapRoleToUser", error);
        }

        return Result.Success;
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
}
