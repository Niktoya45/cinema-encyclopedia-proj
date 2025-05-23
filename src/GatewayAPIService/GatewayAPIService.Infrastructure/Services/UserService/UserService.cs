

using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.RatingDTO;
using Shared.UserDataService.Models.UserDTO;
using Shared.UserDataService.Models.Flags;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.UserService
{
    public class UserService:IUserService
    {
        const string Url = "/api/users";

        HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        { 
            _httpClient = httpClient;
        }

        /* User Get Requests */
        public async Task<UserResponse?> Get(string id, CancellationToken ct)
        {
            return await _httpClient.GetFromJsonAsync<UserResponse>(Url +$"/{id}", ct);
        }
        public async Task<IEnumerable<LabeledResponse>?> GetLabelled(string userId, Label? label, CancellationToken ct)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<LabeledResponse>>(Url+$"/{userId}/label" + (label == null ? "" : $"/{label}"), ct);
        }
        public async Task<RatingResponse?> GetRatingFor(string userId, string cinemaId, CancellationToken ct)
        {
            return await _httpClient.GetFromJsonAsync<RatingResponse>(Url+$"/{userId}/rating/{cinemaId}", ct);
        }

        /******/

        /* User Post Requests */
        public async Task<UserResponse?> Create(CreateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateUserRequest>(Url, user, ct);

            return await response.Content.ReadFromJsonAsync<UserResponse>();
        }
        public async Task<LabeledResponse?> CreateForLabeledList(string userId, CreateLabeledRequest labeled, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateLabeledRequest>(Url+$"/{userId}/label", labeled, ct);

            return await response.Content.ReadFromJsonAsync<LabeledResponse>();
        }
        public async Task<RatingResponse?> CreateForRatingList(string userId, CreateRatingRequest rating, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateRatingRequest>(Url+$"/{userId}/rating", rating, ct);

            return await response.Content.ReadFromJsonAsync<RatingResponse>();
        }

        /******/

        /* User Put Requests */
        public async Task<UserResponse?> Update(string id, UpdateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateUserRequest>(Url+$"/{id}", user, ct);

            return await response.Content.ReadFromJsonAsync<UserResponse>();
        }

        /******/

        /* User Delete Requests */
        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url+$"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFromLabeledList(string userId, string? cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url+$"/{userId}/label" + (cinemaId == null ? "" : $"/{cinemaId}"), ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFromRatingList(string userId, string? cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(Url+$"/{userId}/rating" + (cinemaId == null ? "" : $"/{cinemaId}"), ct);

            return response.IsSuccessStatusCode;
        }

        /******/

    }
}
