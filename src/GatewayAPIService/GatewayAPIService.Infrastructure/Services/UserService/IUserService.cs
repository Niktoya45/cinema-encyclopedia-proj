
using Shared.UserDataService.Models.UserDTO;
using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.RatingDTO;
using Shared.UserDataService.Models.Flags;
using Shared.CinemaDataService.Models.SharedDTO;

namespace GatewayAPIService.Infrastructure.Services.UserService
{
    public interface IUserService
    {
        /* User Get Requests */
        Task<UserResponse?> Get(string id, CancellationToken ct);
        Task<IEnumerable<LabeledResponse>?> GetLabelled(string userId, Label? label, CancellationToken ct);
        Task<IEnumerable<LabeledResponse>?> GetLabelFor(string userId, string cinemaId, CancellationToken ct);
        Task<RatingResponse?> GetRatingFor(string userId, string cinemaId, CancellationToken ct);

        /******/

        /* User Post Requests */
        Task<UserResponse?> Create(CreateUserRequest user, CancellationToken ct);
        Task<LabeledResponse?> CreateForLabeledList(string userId, CreateLabeledRequest labeled, CancellationToken ct);

        /******/

        /* User Put Requests */
        Task<UserResponse?> Update(string id, UpdateUserRequest user, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdatePhoto(string userId, UpdatePictureRequest picture, CancellationToken ct);
        Task<RatingResponse?> UpdateRatingList(string userId, UpdateUserRatingRequest rating, CancellationToken ct);

        /******/

        /* User Delete Requests */
        Task<bool> Delete(string id, CancellationToken ct);
        Task<bool> DeleteFromLabeledList(string userId, string? cinemaId, CancellationToken ct);
        Task<bool> DeleteFromRatingList(string userId, string? cinemaId, CancellationToken ct);

        /******/

    }
}
