using MediatR;
using ImageService.Api.General;
using ImageService.Infrastructure.Models.ImageDTO;
using ImageService.Infrastructure.Models.Flags;
using ImageService.Infrastructure.Repositories;
using ImageService.Api.Requests;
using ImageService.Api.Exceptions;

namespace ImageService.Api.Handlers
{
    public class GetImageHandler : IRequestHandler<GetImage, ImageResponse>
    {
        IImageRepository _repository;
        ImageSettings _settings;
        public GetImageHandler(IImageRepository repository, ImageSettings settings)
        {
            _repository = repository;
            _settings = settings;
        }
        public async Task<ImageResponse> Handle(GetImage request, CancellationToken cancellationToken)
        {
            string subDirectory = request.Size switch
            {
                ImageSize.Tiny => _settings.DirectoryTiny,
                ImageSize.Small => _settings.DirectorySmall,
                ImageSize.Medium => _settings.DirectoryMedium,
                ImageSize.Big => _settings.DirectoryBig,
                ImageSize.Large => _settings.DirectoryLarge,
                _ => ""
            };

            string path = _settings.RootDirectory + subDirectory + request.Id;

            string? uriSAS = await _repository.GetUri(path);

            if (uriSAS is null) 
            {
                throw new ImageNotFoundException();
            }

            return new ImageResponse {Id = request.Id, Uri = uriSAS, Size = request.Size};
        }
    }
}
