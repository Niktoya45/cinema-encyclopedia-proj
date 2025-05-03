

namespace ImageService.Infrastructure.Repositories
{
    public interface IImageRepository
    {
        public Task<string?> GetUri(string path);
        public Task<string?> AddByUrl(string path, Stream image);
        public Task<string?> ReplaceByUrl(string pathOld, string pathNew, Stream image);
        public Task<string?> DeleteByUrl(string path);
    }
}
