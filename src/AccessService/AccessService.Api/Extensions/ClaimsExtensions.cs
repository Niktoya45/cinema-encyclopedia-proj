using AccessService.Domain.Profiles;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace EncyclopediaService.Api.Extensions
{
    public static class ClaimsExtensions
    {
        public static bool DisableAuthentication = true;
        public static bool DisableRoles = true;

        public static bool IsLoggedIn(this ClaimsPrincipal principal)
        {
            return DisableAuthentication || 
                (principal.Identity != null && principal.Identity.IsAuthenticated);
        }

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

        public static ClaimsPrincipal ToPrincipal(this AccessProfileUser user, string authenticationType)
        {
            List<Claim> claims = new();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sid, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Sub));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.AppUsername));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType));
        }
    }
}
