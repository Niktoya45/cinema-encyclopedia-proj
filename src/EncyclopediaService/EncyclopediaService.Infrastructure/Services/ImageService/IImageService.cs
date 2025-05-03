
namespace EncyclopediaService.Infrastructure.Services.ImageService
{
    public interface IImageService
    {
        void AddImage(string id, string imageBase64, ImageSize sizes);
        void ReplaceImage(string id, string imageBase64, ImageSize sizes);
        void DeleteImage(string id, ImageSize sizes);
    }
}
