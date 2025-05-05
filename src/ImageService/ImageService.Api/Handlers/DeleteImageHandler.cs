using ImageService.Api.Exceptions;
using ImageService.Api.General;
using ImageService.Api.Requests;
using ImageService.Infrastructure.Models.Flags;
using ImageService.Infrastructure.Models.ImageDTO;
using ImageService.Infrastructure.Repositories;
using MediatR;

namespace ImageService.Api.Handlers
{
    public class DeleteImageHandler : IRequestHandler<DeleteImage, ImageResponse>
    {
        IImageRepository _repository;
        ImageSettings _settings;
        public DeleteImageHandler(IImageRepository repository, ImageSettings settings)
        {
            _repository = repository;
            _settings = settings;
        }
        public async Task<ImageResponse> Handle(DeleteImage request, CancellationToken cancellationToken)
        {

            Dictionary<ImageSize, string> urisSAS = new Dictionary<ImageSize, string>();

            string path = "";

            string? uriSAS = null;

            foreach (ImageSize size in Enum.GetValues<ImageSize>())
            {
                if ((request.Size & size) == 0)
                {
                    continue;
                }

                path = _settings.RootDirectory + _settings.Directory[size] + request.Id;

                uriSAS = await _repository.DeleteByUrl(path);

                if (uriSAS is null)
                {
                    throw new ImageNotFoundException($"Image {request.Id} was not deleted for size {request.Size}");
                } 
                
                urisSAS.Add(size, uriSAS);
            }

            return new ImageResponse { Id = request.Id, Uris = urisSAS, Size = request.Size };
        }
    }
}
