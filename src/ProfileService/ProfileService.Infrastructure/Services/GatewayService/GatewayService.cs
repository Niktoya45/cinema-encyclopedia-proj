
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.ImageDTO;
using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.UserDTO;
using Shared.UserDataService.Models.Flags;
using ProfileService.Infrastructure.Extensions;
using System.Net.Http.Json;

namespace ProfileService.Infrastructure.Services.GatewayService
{
    public class GatewayService:IGatewayService
    {
        const string usersUri = "/api/users";

        HttpClient _httpClient;
        public GatewayService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ***    USER API   *** //
        // *********************** //

        /* User GET Requests */
        public async Task<UserResponse?> GetUser(string id, CancellationToken ct)
        { 
            var response = await _httpClient.GetAsync(usersUri + $"/{id}", ct);

            return await response.HandleResponse<UserResponse>();
        }
        public async Task<LabeledCinemasResponse<CinemasResponse>?> GetUserLabeled(string userId, Label? label, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri + $"/{userId}/label/{label}", ct);

            return await response.HandleResponse<LabeledCinemasResponse<CinemasResponse>?>();
        }
        public async Task<string?> GetUserRole(string userId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri + $"/{userId}/role", ct);

            return await response.Content.ReadAsStringAsync();
        }

        /******/

        /* User PUT Requests */
        public async Task<UserResponse?> UpdateUser(string id, UpdateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateUserRequest>(usersUri + $"/{id}", user, ct);

            return await response.HandleResponse<UserResponse>();
        }
        public async Task<string?> AddUserRole(string id, string role, CancellationToken ct)
        {
            var response = await _httpClient.PutAsync(usersUri + $"/{id}/grant-role?role={role}", null, ct);

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string?> RemoveUserRole(string id, string role, CancellationToken ct)
        {
            var response = await _httpClient.PutAsync(usersUri + $"/{id}/revoke-role?role={role}", null, ct);

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<UpdatePictureResponse?> UpdateUserPhoto(string userId, ReplaceImageRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<ReplaceImageRequest>(usersUri + $"/{userId}/picture/update", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>();
        }

        /******/

        /* User DELETE Requests */

        public async Task<bool> Delete(string userId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri + $"/{userId}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFromLabeledList(string userId, string cinemaId, Label label, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri + $"/{userId}/label/{cinemaId}" + (label == Label.None ? "" : $"/{label}"), ct);

            return response.IsSuccessStatusCode;
        }

        /******/

        // *********************** //
        // ***    USER API   *** //
    }
}
