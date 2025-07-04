
using GatewayAPIService.Infrastructure.Extensions;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.AccessService
{
    public class AccessService:IAccessService
    {
        const string accessUri = "manage";

        HttpClient _httpClient;
        public AccessService(HttpClient httpClient)
        { 
            _httpClient = httpClient;
        }

        /* Subject Get Requests */

        public async Task<string?> GetRole(string userId, CancellationToken ct)
        { 
            var response = await _httpClient.GetAsync(accessUri + $"/{userId}/role", ct);

            return await response.Content.ReadAsStringAsync();
        }

        /******/

        /* Subject Put Requests */

        public async Task<string?> GrantRole(string userId, string role, CancellationToken ct)
        {
            var response = await _httpClient.PutAsync(accessUri + $"/{userId}/grant-role?rolename={role}",null, ct);

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string?> RevokeRole(string userId, string role, CancellationToken ct)
        {
            var response = await _httpClient.PutAsync(accessUri + $"/{userId}/revoke-role?rolename={role}",null, ct);

            return await response.Content.ReadAsStringAsync();
        }

        /******/

        /* Subject Delete Requests */

        public async Task<bool> DeleteProfile(string userId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(accessUri + $"/{userId}/delete-profile", ct);

            return response.IsSuccessStatusCode;
        }

        /******/
    }
}
