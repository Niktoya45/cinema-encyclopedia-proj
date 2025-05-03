
namespace EncyclopediaService.Infrastructure.Services.ImageService
{
    public class ImageService:IImageService
    {

        HttpClient _imageHttpClient;

        public ImageService(HttpClient imageHttpClient) { 
            _imageHttpClient = imageHttpClient;
        }
        public void AddImage(string id, string imageBase64, ImageSize sizes) { 
            //implement 
        }
        public void ReplaceImage(string id, string imageBase64, ImageSize sizes) { 
            //implement
        }
        public void DeleteImage(string id, ImageSize sizes) { 
            //implement
        }
    }
}
