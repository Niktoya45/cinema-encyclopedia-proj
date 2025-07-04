

using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System.Runtime;
using static System.Net.Mime.MediaTypeNames;

namespace ImageService.Infrastructure.Repositories
{
    public class AzureImageRepository:IImageRepository
    {
        BlobContainerClient _containerClient;

        DateTimeOffset readOffset = DateTimeOffset.UtcNow.AddHours(2);
        public AzureImageRepository(BlobContainerClient containerClient) 
        {
            _containerClient = containerClient;
        }

        public string GetBlobUri(string path)
        {
            Uri uri = _containerClient.GetBlobClient(path).GenerateSasUri(BlobSasPermissions.Read, readOffset);

           return uri.AbsoluteUri;
        }

        public async Task<string?> GetUri(string path) 
        {
            var res = await _containerClient.GetBlobClient(path).ExistsAsync();

            if (!res.Value)
            {
                return null;
            }

            string uri = GetBlobUri(path.Substring(1));

            return uri;
        }
        public async Task<string?> AddByUrl(string path, Stream image) 
        {
            if (image is null)
                return null;


            var res = await _containerClient.UploadBlobAsync(path, image);

            if (res.GetRawResponse().IsError) 
            {
                return null;
            }

            string uri = GetBlobUri(path);

            return uri;
        }
        public async Task<string?> ReplaceByUrl(string pathOld, string pathNew, Stream image)
        {
            if (image is null)
                return null;

            var res = await _containerClient.DeleteBlobIfExistsAsync(pathOld);

            return await AddByUrl(pathNew, image);
        }
        public async Task<string?> DeleteByUrl(string path)
        {
            var res = await _containerClient.DeleteBlobIfExistsAsync(path);

            if (!res.Value)
            {
                return null;
            }

            return path;
        }
    }
}
