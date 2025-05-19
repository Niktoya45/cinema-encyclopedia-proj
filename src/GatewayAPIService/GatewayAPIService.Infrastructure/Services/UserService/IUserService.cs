
using Shared.UserDataService.Models.UserDTO;
using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.RatingDTO;
using Shared.UserDataService.Models.Flags;

namespace GatewayAPIService.Infrastructure.Services.UserService
{
    public interface IUserService
    {
        /* User Get Requests */
        Task<UserResponse> Get(string id);
        Task<UserResponse> GetLabelled(string userId, Label? label);
        Task<UserResponse> GetRatingFor(string userId, string cinemaId);

        /******/

        /* User Post Requests */
        Task<UserResponse> Create(CreateUserRequest user);
        Task<LabeledResponse> CreateForLabeledList(CreateLabeledRequest labeled);
        Task<RatingResponse> CreateForRatingList(CreateRatingRequest rating);

        /******/

        /* User Put Requests */
        Task<UserResponse> Update(string id, UpdateUserRequest user);

        /******/

        /* User Delete Requests */
        Task Delete(string id);
        Task DeleteFromLabeledList(string userId, string cinemaId);
        Task DeleteFromRatingList(string userId, string cinemaId);

        /******/

    }
}
