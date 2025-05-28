
using AccessService.Api.Config;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace AccessService.Api.Extensions
{
    public static class JsonWebKeyConfig 
    {
        public static void AddJsonWebKeys(this IServiceCollection services, string filename)
        {

            const string _signatureFile = "signature.rsa";

            JsonWebKeySet webkeyset;
            RsaSecurityKey sign;
            string webkeysetJson;

            FileInfo keyfile = new FileInfo(filename);

            if (!keyfile.Exists)
            {
                RSA rsa = RSA.Create(2048);

                using (FileStream s = keyfile.Create())
                {
                    RsaSecurityKey rsakey = new (rsa.ExportParameters(false)){ KeyId = "key1"};

                    JsonWebKey webkey = JsonWebKeyConverter.ConvertFromRSASecurityKey(rsakey);

                    Dictionary<string, IList<JsonWebKey>> webkeyDict = new()
                    {
                        { "keys", new List<JsonWebKey>{ webkey } }
                    };

                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        webkeysetJson = webkeyDict.ToJson();

                        sw.WriteLine(webkeysetJson);
                    }
                }

                sign = new(rsa.ExportParameters(true)) { KeyId = "key1" };

                using (FileStream ss = new FileInfo(_signatureFile).Create()) 
                {
                    using (StreamWriter ssw = new StreamWriter(ss))
                    {
                        ssw.WriteLine(JsonWebKeyConverter.ConvertFromRSASecurityKey(sign).ToJson());
                    }
                }

            }
            else
            {
                using (StreamReader sr = keyfile.OpenText())
                {
                    webkeysetJson = sr.ReadToEnd();
                }
                string signJson;
                using (StreamReader ssr = new FileInfo(_signatureFile).OpenText())
                {
                    signJson = ssr.ReadToEnd();
                }

                JsonWebKey websign;

                try
                {
                    websign = JsonWebKey.Create(signJson);
                }
                catch (ArgumentException ae)
                {
                    throw new Exception($"Signature in file \'{_signatureFile}\' was not in correct format or is missing");
                }

                sign = new RsaSecurityKey(CreateRsaParameters(websign));
            }

            try
            {
                webkeyset = new JsonWebKeySet(webkeysetJson);
            }
            catch (ArgumentException ae)
            {
                throw new Exception($"Keys in file \'{filename}\' were not in correct format or are missing");
            }

            services.AddSingleton<JWKConfig>(config =>
            {
                return new JWKConfig
                {
                    JWKSFile = filename,
                    JWKS = webkeyset,
                    Signature = sign
                };
            });
        }

        internal static RSAParameters CreateRsaParameters(JsonWebKey jwk)
        {
            if (string.IsNullOrEmpty(jwk.N))
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10700: {0} is unable to use 'rsaParameters'. {1} is null.", LogHelper.MarkAsNonPII("Microsoft.IdentityModel.Tokens.JsonWebKey"), LogHelper.MarkAsNonPII("Modulus"))));
            }

            if (string.IsNullOrEmpty(jwk.E))
            {
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10700: {0} is unable to use 'rsaParameters'. {1} is null.", LogHelper.MarkAsNonPII("Microsoft.IdentityModel.Tokens.JsonWebKey"), LogHelper.MarkAsNonPII("Exponent"))));
            }

            RSAParameters result = default(RSAParameters);
            result.Modulus = Base64UrlEncoder.DecodeBytes(jwk.N);
            result.Exponent = Base64UrlEncoder.DecodeBytes(jwk.E);
            result.D = (string.IsNullOrEmpty(jwk.D) ? null : Base64UrlEncoder.DecodeBytes(jwk.D));
            result.P = (string.IsNullOrEmpty(jwk.P) ? null : Base64UrlEncoder.DecodeBytes(jwk.P));
            result.Q = (string.IsNullOrEmpty(jwk.Q) ? null : Base64UrlEncoder.DecodeBytes(jwk.Q));
            result.DP = (string.IsNullOrEmpty(jwk.DP) ? null : Base64UrlEncoder.DecodeBytes(jwk.DP));
            result.DQ = (string.IsNullOrEmpty(jwk.DQ) ? null : Base64UrlEncoder.DecodeBytes(jwk.DQ));
            result.InverseQ = (string.IsNullOrEmpty(jwk.QI) ? null : Base64UrlEncoder.DecodeBytes(jwk.QI));
            return result;

        }
    }
}
