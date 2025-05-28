using Microsoft.IdentityModel.Tokens;

namespace AccessService.Api.Config
{
    public class JWKConfig
    {
        public JsonWebKeySet JWKS { get; set; }

        public string JWKSFile { get; set; }

        public RsaSecurityKey Signature { get; set; }
    }
}
