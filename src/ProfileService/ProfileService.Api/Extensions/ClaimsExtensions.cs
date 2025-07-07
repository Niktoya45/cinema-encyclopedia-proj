using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace ProfileService.Api.Extensions
{
    public static class ClaimsExtensions
    {
        public static bool DisableAuthentication = false;
        public static bool DisableRoles = false;
        public static bool DisablePosession = false;

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
            (role != null && role.Value.Contains("dministrator"));

            return IsAdmin;
        }

        public static bool IsSuperAdmin(this ClaimsPrincipal principal) 
        {
            var Role = principal.FindFirst("role");

            bool IsAdmin = DisableRoles ||
                (Role != null && Role.IsSuperAdmin());

            return IsAdmin;
        }

        public static bool IsSuperAdmin(this Claim role)
        {
            bool IsAdmin = DisableRoles ||
            (role != null && role.Value.Contains("Superadministrator"));

            return IsAdmin;
        }
        
        public static bool IsProfileOwner(this ClaimsPrincipal principal, string profileId)
        {
            return DisablePosession || 
                principal.FindFirstValue(JwtRegisteredClaimNames.Sub) == profileId;
        }

        public static ClaimsPrincipal ToPrincipal(Dictionary<string, string> claimsDictionary, string authenticationType)
        {
            List<Claim> claims = new List<Claim>();

            foreach (var c in claimsDictionary)
            {
                claims.Add(new Claim(c.Key, c.Value));
            }

            return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType));
        }


        public static bool AddJwtTokenIdentity(this ClaimsPrincipal principal, string jwtToken)
        {
            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();

            JsonWebToken token;

            try
            {
                token = tokenHandler.ReadJsonWebToken(jwtToken);
            }
            catch (Exception)
            {
                return false;
            }

            var claims = token.Claims;

            principal.AddIdentity(new ClaimsIdentity(claims.Append(new Claim("token", jwtToken)), "cookie"));

            return true;
        }

        public static async Task SignInByCookies(this ClaimsPrincipal principal, HttpContext context, bool isPersistent)
        {

            AuthenticationProperties props = null;
            if (isPersistent)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(45)
                };
            }
            ;

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
        }

    }
}
