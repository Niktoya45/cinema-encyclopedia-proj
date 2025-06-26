using Azure;
using Shared.UserDataService.Models.UserDTO;
using System.Net.Http;
using System.Net.Http.Json;

namespace AccessService.Infrastructure.Services
{
    public class UserService:IUserService
    {
        const string usersUri = "/api/users";

        HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ***    USER API   *** //
        // *********************** //

        /* User GET Requests */
        public async Task<UserResponse?> GetUser(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri + $"/{id}", ct);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }

            return null;
        }

        /******/

        /* User POST Requests */
        public async Task<UserResponse?> CreateUser(CreateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync(usersUri, user, ct);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }

            return null;
        }

        /******/

        /* User PUT Requests */
        public async Task<UserResponse?> UpdateUser(string id, UpdateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(usersUri, user, ct);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }

            return null;
        }

        /******/

        /* User DELETE Requests */
        public async Task<bool> DeleteUser(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri + $"/{id}", ct);

            return response.IsSuccessStatusCode;
        }

        /******/

        // *********************** //
        // ***    USER API   *** //        
    }
}
