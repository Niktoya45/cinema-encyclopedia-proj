using AccessService.Api.Config;
using AccessService.Domain.Profiles;
using Azure;
using Azure.Core;
using EncyclopediaService.Api.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Mono.TextTemplating;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AccessService.Api.Controllers
{
    [Route("oidc")]
    public class OIDCController : Controller
    {
        const string _provider = "https://localhost:7156/oidc";

        private readonly UserManager<AccessProfileUser> _userManager;
        JWKConfig _jwkConfig;
        HttpClient _client;

        public OIDCController(JWKConfig jwkConfig, UserManager<AccessProfileUser> userManager)
        {
            _userManager = userManager;
            _jwkConfig = jwkConfig;
            _client = new HttpClient();
        }

        [Route("userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();

            string? authorization = Request.Headers["Authorization"][0];

            string? atoken = authorization is null ? null : (authorization).Split()[1];

            JsonWebToken token = tokenHandler.ReadJsonWebToken(atoken);

            var tcls = token.Claims;

            Claim? Subject = tcls.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            Claim? Sid = tcls.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            Claim? Iss = new Claim(JwtRegisteredClaimNames.Iss, _provider);

            return new JsonResult(new {
                    sub = Subject is null ? "test" : Subject.Value,
                    sid = Sid is null ? "test" : Sid.Value
                });
        }

        [Route("token")]
        public async Task<IActionResult> Token(string code)
        {
            string sub = code.Split('.')[1];

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Sub == sub);

            if (user is null)
            {
                return new UnauthorizedObjectResult(sub);
            }

            if(!User.Identities.Any())
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user.ToPrincipal("cookie"), new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15)
                });

            var jwts = await _userManager.GetAuthenticationTokenAsync(user, nameof(AccessService), code);

            if (jwts is null)
            {
                return new UnauthorizedObjectResult(user.Sub);
            }

            var response = new
            {
                access_token = jwts,
                id_token = jwts,
                token_type = "Bearer",
                expires_in = 300
            };

            return new JsonResult(response);

        }

        [Route("authorize")]
        public async Task<IActionResult> Authorize()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { ReturnUrl = Request.Path + Request.QueryString });
            }

            string redirect = Request.Query["redirect_uri"];

            string state = Request.Query["state"];
            string nonce = Request.Query["nonce"];

            string sub = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            string code = Guid.NewGuid().ToString() + "." + sub;

            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();

            Dictionary<string, object> ClaimDictionary = new Dictionary<string, object>();

            foreach (Claim c in User.Claims)
            {
                ClaimDictionary.Add(c.Type, c.Value);
            }

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new List<Claim>{
                        User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)??new Claim(JwtRegisteredClaimNames.Sub, "test"),
                        User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)??new Claim(JwtRegisteredClaimNames.Sid, "test")
                    }),
                Claims = ClaimDictionary,
                Issuer = _provider,
                IssuedAt = DateTime.UtcNow,
                Audience = "cinema_encyclopedia",
                Expires = DateTime.Now.AddMinutes(45),
                SigningCredentials = new SigningCredentials(_jwkConfig.Signature, SecurityAlgorithms.RsaSsaPssSha256)
            };

            string jwts = tokenHandler.CreateToken(descriptor);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Sub == sub);

            if(user != null)
                await _userManager.SetAuthenticationTokenAsync(
                    user, nameof(AccessService), code, jwts
                    );


            return Redirect(redirect+$"?code={code}&nonce={nonce}&state={state}");
        }

        [Route("jwk")]
        public async Task JWKS() 
        {
            await Response.SendFileAsync(_jwkConfig.JWKSFile);
        }

        [Route(".well-known/openid-configuration")]
        public async Task<IActionResult> Discovery()
        {
            return Ok(
                new
                {
                    issuer = _provider,
                    authorization_endpoint = _provider + "/authorize",
                    token_endpoint = _provider + "/token",
                    introspection_endpoint = _provider + "/token/introspect",
                    revocation_endpoint = _provider + "/token/revoke",
                    userinfo_endpoint = _provider + "/userinfo",
                    end_session_endpoint = _provider + "/end_session",
                    jwks_uri = _provider + "/jwk",
                    request_parameter_supported = true,
                    request_uri_parameter_supported = true,

                    response_types_supported = new string[] {
                  "code",
                  "id_token",
                  "token",
                  "none",
                  "code id_token",
                  "code token",
                  "id_token token",
                  "code id_token token"
                },

                    id_token_signing_alg_values_supported = new string[] {
                  "RS256"
                },

                    grant_types_supported = new string[] {
                  "authorization_code",
                  "client_credentials",
                  "implicit",
                  "password",
                  "refresh_token"
                },

                    scopes_supported = new string[] {
                  "openid",
                  "profile",
                  "email",
                  "address",
                  "phone",
                },

                    claims_parameter_supported = true,
                    claims_supported = new string[]{
                  "sub",
                  "iss",
                  "auth_time",
                  "name",
                  "username",
                  "preferred_username",
                  "gender",
                  "birthdate",
                  "updated_at",
                  "phone_number",
                  "phone_number_verified",
                  "email",
                  "email_verified"
                },

                    code_challenge_methods_supported = new string[] {
                  "S256"
                },

                }    
            );
        }
    }
}
