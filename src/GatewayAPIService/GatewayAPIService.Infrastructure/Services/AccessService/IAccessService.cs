
namespace GatewayAPIService.Infrastructure.Services.AccessService
{
    public interface IAccessService
    {
        /* Subject Get Requests */

        Task<string?> GetRole(string userId, CancellationToken ct);

        /******/

        /* Subject Put Requests */

        Task<string?> GrantRole(string userId, string role, CancellationToken ct);
        Task<string?> RevokeRole(string userId, string role, CancellationToken ct);

        /******/

        /* Subject Delete Requests */

        Task<bool> DeleteProfile(string userId, CancellationToken ct);
             
        /******/
    }
}
