
using System.Net.Http.Json;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;

namespace GatewayAPIService.Infrastructure.Services.ImageService
{
    public class ImageService:IImageService
    {
        const string imagesUri = "/api/images";

        HttpClient _imageHttpClient;

        public ImageService(HttpClient imageHttpClient) { 
            _imageHttpClient = imageHttpClient;
        }
        public async Task<string?> GetImage(string id, ImageSize sizes) 
        {
            var response = await _imageHttpClient.GetFromJsonAsync<ImageResponse>(imagesUri + $"?id={id}&size={sizes}");

            return response is null ? null : response.Uris[sizes];
        }
        public async Task<string?> AddImage(string id, string imageBase64, ImageSize sizes, ImageSize targetSize) 
        {

            AddImageRequest payload = new AddImageRequest { Id = id, FileBase64 = imageBase64, Size = sizes};

            var response = await _imageHttpClient.PostAsJsonAsync<AddImageRequest>(imagesUri, payload);

            if (response.IsSuccessStatusCode)
            {
                ImageResponse? responseImage = await response.Content.ReadFromJsonAsync<ImageResponse>();

                return responseImage is null ? null : responseImage.Uris[targetSize];
            }

            return null;
        }
        public async Task<string?> ReplaceImage(string id, string newId, string imageBase64, ImageSize sizes, ImageSize targetSize) 
        {

            ReplaceImageRequest payload = new ReplaceImageRequest { Id = id, NewId = newId, FileBase64 = imageBase64, Size = sizes };

            var response = await _imageHttpClient.PutAsJsonAsync<ReplaceImageRequest>(imagesUri, payload);

            if (response.IsSuccessStatusCode)
            {
                ImageResponse? responseImage = await response.Content.ReadFromJsonAsync<ImageResponse>();

                return responseImage is null ? null : responseImage.Uris[targetSize];
            }

            return null;

        }
        public async Task<string?> DeleteImage(string id, ImageSize sizes)
        {

            DeleteImageRequest payload = new DeleteImageRequest { Id = id, Size = sizes };

            var response = await _imageHttpClient.DeleteAsync($"images?id={payload.Id}&size={(int)payload.Size}");

            if (response.IsSuccessStatusCode)
            {
                ImageResponse? body = await response.Content.ReadFromJsonAsync<ImageResponse>() ?? throw new Exception("Achtung11! Null body on image DELETE :(");

                return body.Id;
            }

            return null;
        }
    }
}
