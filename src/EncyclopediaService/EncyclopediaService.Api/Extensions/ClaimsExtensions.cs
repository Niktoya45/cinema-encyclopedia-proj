using System.Data;
using System.Security.Claims;

namespace EncyclopediaService.Api.Extensions
{
    public static class ClaimsExtensions
    {
        public static bool DisableRoles = true;
        public static bool IsAdmin(this ClaimsPrincipal principal) 
        {
            var Role = principal.FindFirst("role");

            bool IsAdmin = DisableRoles ||
                (Role != null && Role.IsAdmin());

            return IsAdmin;
        }

        public static bool IsAdmin(this Claim role)
        {
            bool IsAdmin = DisableRoles ||
            (role != null && (role.Value.Contains("Administrator") || role.Value.Contains("Superadministrator")));

            return IsAdmin;
        }
    }
}
