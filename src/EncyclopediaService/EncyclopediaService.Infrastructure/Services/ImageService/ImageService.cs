
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;

namespace EncyclopediaService.Infrastructure.Services.ImageService
{
    public class ImageService:IImageService
    {

        HttpClient _imageHttpClient;

        public ImageService(HttpClient imageHttpClient) { 
            _imageHttpClient = imageHttpClient;
        }
        public async Task<string?> AddImage(string id, string imageBase64, ImageSize sizes) 
        {
            //implement 

            AddImageRequest payload = new AddImageRequest { Id = id, FileBase64 = imageBase64, Size = sizes};

            var response = await _imageHttpClient.PostAsJsonAsync<AddImageRequest>("images", payload);

            if (response.IsSuccessStatusCode)
            {
                ImageResponse? body = await response.Content.ReadFromJsonAsync<ImageResponse>()??throw new Exception("Achtung11! Null body on image POST :(");

                return body.Id;
            }

            return null;
        }
        public async Task<string?> ReplaceImage(string id, string newId, string imageBase64, ImageSize sizes) 
        {
            //implement

            ReplaceImageRequest payload = new ReplaceImageRequest { Id = id, NewId = newId, FileBase64 = imageBase64, Size = sizes };

            var response = await _imageHttpClient.PutAsJsonAsync<ReplaceImageRequest>("images", payload);

            if (response.IsSuccessStatusCode)
            {
                ImageResponse? body = await response.Content.ReadFromJsonAsync<ImageResponse>() ?? throw new Exception("Achtung11! Null body on image PUT :(");

                return body.Id;
            }

            return null;

        }
        public async Task<string?> DeleteImage(string id, ImageSize sizes)
        {
            //implement

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
