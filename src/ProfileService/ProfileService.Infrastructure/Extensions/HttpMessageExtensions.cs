
using System.Net.Http.Json;

namespace ProfileService.Infrastructure.Extensions
{
    public static class HttpMessageExtensions
    {
        public static async Task<T?> HandleResponse<T>(this HttpResponseMessage response) where T : class?
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }

            return null;
        }
    }
}
