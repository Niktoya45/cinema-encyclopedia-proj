
using Shared.UserDataService.Models.UserDTO;

namespace AccessService.Infrastructure.Services
{
    public interface IUserService
    {
        // ***    USER API   *** //
        // *********************** //

        /* User GET Requests */
        Task<UserResponse?> GetUser(string id, CancellationToken ct);

        /******/

        /* User POST Requests */
        Task<UserResponse?> CreateUser(CreateUserRequest user, CancellationToken ct);

        /******/

        /* User PUT Requests */
        Task<UserResponse?> UpdateUser(string id, UpdateUserRequest user, CancellationToken ct);

        /******/

        /* User DELETE Requests */
        Task<bool> DeleteUser(string id, CancellationToken ct);

        /******/

        // *********************** //
        // ***    USER API   *** //
    }
}
