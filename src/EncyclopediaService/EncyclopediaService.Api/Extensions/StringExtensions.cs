using System.Text;
using System.Security.Cryptography;

namespace EncyclopediaService.Api.Extensions
{
    public static class StringExtensions
    {
        public static string SHA_1(this string value)
        {
            StringBuilder Sb = new StringBuilder();

            Encoding enc = Encoding.UTF8;
            byte[] result = SHA1.HashData(enc.GetBytes(value));

           foreach (byte b in result)
               Sb.Append(b.ToString("x2"));

            return Sb.ToString();
        }
    }
}
