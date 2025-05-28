using AccessService.Api.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using static System.Net.WebRequestMethods;

namespace AccessService.Api.Controllers
{
    [Route("oidc")]
    public class OIDCController : Controller
    {
        const string _provider = "https://localhost:7156/oidc";

        JWKConfig _jwkConfig;

        public OIDCController(JWKConfig jwkConfig)
        {
            _jwkConfig = jwkConfig;
        }

        [Route("authorize")]
        public async Task<IActionResult> Authorize()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { ReturnUrl = Request.Path+Request.QueryString });
            }

            string redirect = Request.Query["redirect_uri"];

            string other = Request.Query.Aggregate("?", (acc, p) => acc + (p.Key == "redirect_uri" ? "" : $"&{p.Key}={p.Value}"));

            return Redirect(redirect+other);
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
                  "given_name",
                  "address",
                  "family_name",
                  "middle_name",
                  "preferred_username",
                  "gender",
                  "birthdate",
                  "updated_at",
                  "phone_number",
                  "phone_number_verified",
                  "email",
                  "email_verified",
                  "sid"
                },

                    code_challenge_methods_supported = new string[] {
                  "S256"
                },

                }    
            );
        }
    }
}
