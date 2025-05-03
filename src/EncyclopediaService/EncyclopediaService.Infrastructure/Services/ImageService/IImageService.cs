
using Shared.ImageService.Models.Flags;

namespace EncyclopediaService.Infrastructure.Services.ImageService
{
    public interface IImageService
    {
        Task<string?> AddImage(string imageName, string imageBase64, ImageSize sizes);
        Task<string?> ReplaceImage(string id, string imageName, string imageBase64, ImageSize sizes);
        Task<string?> DeleteImage(string id, ImageSize sizes);
    }
}
