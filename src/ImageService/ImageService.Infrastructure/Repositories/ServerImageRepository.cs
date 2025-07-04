
using ImageService.Infrastructure.Environment;

namespace ImageService.Infrastructure.Repositories
{
    public class ServerImageRepository:IImageRepository
    {
        WebContentEnv _webEnv { get; init; }
        public ServerImageRepository(WebContentEnv webEnv)
        { 
            _webEnv = webEnv;
        }
        public async Task<string?> GetUri(string path)
        {
            return _webEnv.Host + "/images/" + path;
        }
        public async Task<string?> AddByUrl(string path, Stream image)
        {
            string? imageUrl = null;

            if (image != null)
            {
                string pathRoute = _webEnv.Root + $"/images/{path}".Replace('/', '\\');

                using (var stream = new FileStream(pathRoute, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrl = _webEnv.Host + "/images/" + path;
            }

            return imageUrl;
        }
        public async Task<string?> ReplaceByUrl(string pathOld, string pathNew, Stream image)
        {

            if (!string.IsNullOrEmpty(pathOld))
            {
                var pathOldRoute = _webEnv.Root + $"/images/{pathOld}".Replace('/', '\\');

                if (System.IO.File.Exists(pathOldRoute))
                {
                    System.IO.File.Delete(pathOldRoute);
                }
            }

            string pathNewRoute = _webEnv.Root + $"/images/{pathNew}".Replace('/', '\\');

            using (var stream = new FileStream(pathNewRoute, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            string imageUrl = _webEnv.Host + "/images/" + pathNew;

            return imageUrl;
        }
        public async Task<string?> DeleteByUrl(string path)
        {
            var pathRoute = _webEnv.Root + $"/images/{path}".Replace('/', '\\');

            if (System.IO.File.Exists(pathRoute))
            {
                System.IO.File.Delete(pathRoute);
            }

            return path;
        }
    }
}
