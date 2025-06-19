

using System.Net.Http.Json;

namespace GatewayAPIService.Infrastructure.Extensions
{
    public static class HttpMessageExtensions
    {
        public static async Task<T?> HandleResponse<T>(this HttpResponseMessage response, CancellationToken ct = default) where T : class?
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>(ct);
            }

            return null;
        }
    }
}
