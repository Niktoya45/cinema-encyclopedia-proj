
using Shared.ImageService.Models.Flags;

namespace GatewayAPIService.Infrastructure.Services.ImageService
{
    public interface IImageService
    {
        Task<string?> GetImage(string id, ImageSize sizes);
        Task<string?> AddImage(string id, string imageBase64, ImageSize sizes);
        Task<string?> ReplaceImage(string id, string newId, string imageBase64, ImageSize sizes);
        Task<string?> DeleteImage(string id, ImageSize sizes);
    }
}
