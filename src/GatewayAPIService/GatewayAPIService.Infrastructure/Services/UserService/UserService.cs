

using GatewayAPIService.Infrastructure.Extensions;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.UserDataService.Models.Flags;
using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.RatingDTO;
using Shared.UserDataService.Models.UserDTO;
using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Services.UserService
{
    public class UserService:IUserService
    {
        const string usersUri = "/api/users";

        HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        { 
            _httpClient = httpClient;
        }

        /* User Get Requests */
        public async Task<UserResponse?> Get(string id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri +$"/{id}", ct);

            return await response.HandleResponse<UserResponse>(ct);
        }
        public async Task<IEnumerable<LabeledResponse>?> GetLabelled(string userId, Label? label, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri+$"/{userId}/label" + (label == null ? "" : $"/{label}"), ct);

            return await response.HandleResponse<IEnumerable<LabeledResponse>>(ct);
        }
        public async Task<IEnumerable<LabeledResponse>?> GetLabelFor(string userId, string cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri+$"/{userId}/label?cinemaId={cinemaId}", ct);

            return await response.HandleResponse<IEnumerable<LabeledResponse>>(ct);
        }
        public async Task<RatingResponse?> GetRatingFor(string userId, string cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(usersUri+$"/{userId}/rating/{cinemaId}", ct);

            return await response.HandleResponse<RatingResponse>(ct);
        }

        /******/

        /* User Post Requests */
        public async Task<UserResponse?> Create(CreateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateUserRequest>(usersUri, user, ct);

            return await response.HandleResponse<UserResponse>(ct);
        }
        public async Task<LabeledResponse?> CreateForLabeledList(string userId, CreateLabeledRequest labeled, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateLabeledRequest>(usersUri+$"/{userId}/label", labeled, ct);

            return await response.HandleResponse<LabeledResponse>(ct);
        }

        /******/

        /* User Put Requests */
        public async Task<UserResponse?> Update(string id, UpdateUserRequest user, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateUserRequest>(usersUri+$"/{id}", user, ct);

            return await response.HandleResponse<UserResponse>(ct);
        }

        public async Task<UpdatePictureResponse?> UpdatePhoto(string userId, UpdatePictureRequest picture, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdatePictureRequest>(usersUri + $"/{userId}/picture", picture, ct);

            return await response.HandleResponse<UpdatePictureResponse>();
        }
        public async Task<RatingResponse?> UpdateRatingList(string userId, UpdateUserRatingRequest rating, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync(usersUri + $"/{userId}/rating", rating, ct);

            return await response.HandleResponse<RatingResponse>(ct);
        }

        /******/

        /* User Delete Requests */
        public async Task<bool> Delete(string id, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri+$"/{id}", ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFromLabeledList(string userId, string? cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri+$"/{userId}/label" + (cinemaId == null ? "" : $"/{cinemaId}"), ct);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteFromRatingList(string userId, string? cinemaId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync(usersUri+$"/{userId}/rating" + (cinemaId == null ? "" : $"/{cinemaId}"), ct);

            return response.IsSuccessStatusCode;
        }

        /******/

    }
}
