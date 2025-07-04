
using System.Net.Http.Json;

namespace EncyclopediaService.Infrastructure.Extensions
{
    public static class HttpMessageExtensions
    {
        public static async Task<T?> HandleResponse<T>(this HttpResponseMessage response) where T : class?
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return null;
                return await response.Content.ReadFromJsonAsync<T>();
            }

            return null;
        }
    }
}
