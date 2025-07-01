using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.UserDTO;
using Shared.ImageService.Models.ImageDTO;
using Shared.UserDataService.Models.Flags;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace ProfileService.Infrastructure.Services.GatewayService
{
    public interface IGatewayService
    {
        // ***    USER API   *** //
        // *********************** //

        /* User GET Requests */
        Task<UserResponse?> GetUser(string id, CancellationToken ct);
        Task<LabeledCinemasResponse<CinemasResponse>?> GetUserLabeled(string userId, Label? label, CancellationToken ct);
        Task<string?> GetUserRole(string userId, CancellationToken ct);

        /******/

        /* User PUT Requests */
        Task<UserResponse?> UpdateUser(string id, UpdateUserRequest user, CancellationToken ct);
        Task<string?> AddUserRole(string id, string user, CancellationToken ct);
        Task<string?> RemoveUserRole(string id, string user, CancellationToken ct);
        Task<UpdatePictureResponse?> UpdateUserPhoto(string userId, ReplaceImageRequest picture, CancellationToken ct);

        /******/

        /* User DELETE Requests */
        Task<bool> DeleteFromLabeledList(string userId, string cinemaId, CancellationToken ct);

        /******/

        // *********************** //
        // ***    USER API   *** //
    }
}
