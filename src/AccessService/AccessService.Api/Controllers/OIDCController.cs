using AccessService.Api.Config;
using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Text;
using Mono.TextTemplating;

namespace AccessService.Api.Controllers
{
    [Route("oidc")]
    public class OIDCController : Controller
    {
        const string _provider = "https://localhost:7156/oidc";

        JWKConfig _jwkConfig;
        HttpClient _client;

        public OIDCController(JWKConfig jwkConfig)
        {
            _jwkConfig = jwkConfig;
            _client = new HttpClient();
        }

        [Route("userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            Claim? Subject = User.Claims.FirstOrDefault(c => c.Type == "sub");

            return new JsonResult(new {
                sub = Subject is null ? "" : Subject.Value
            });
        }

        [Route("token")]
        public async Task<IActionResult> Token()
        {
            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();

            Dictionary<string, object> ClaimDictionary = new Dictionary<string, object>();

            foreach(Claim c in User.Claims)
            {
                ClaimDictionary.Add(c.Type, c.Value);
            }

            IEnumerable<ClaimsIdentity> claimsi = User.Identities;

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>{ User.Claims.FirstOrDefault(c => c.Type == "sub")??new Claim("sub", "") }),
                Claims = ClaimDictionary,
                Issuer = _provider,
                IssuedAt = DateTime.UtcNow,
                Audience = "cinema_encyclopedia",
                Expires = DateTime.Now.AddMinutes(45),
                SigningCredentials = new SigningCredentials(_jwkConfig.Signature, SecurityAlgorithms.RsaSsaPssSha256)
            };

            string jwts = tokenHandler.CreateToken(descriptor);

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
                return RedirectToPage("/Account/Login", new { ReturnUrl = Request.Path+Request.QueryString });
            }

            string redirect = Request.Query["redirect_uri"];

            string state = Request.Query["state"];
            string nonce = Request.Query["nonce"];

            string code = Guid.NewGuid().ToString();

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
