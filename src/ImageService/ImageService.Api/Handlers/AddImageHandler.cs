using ImageService.Api.Exceptions;
using ImageService.Api.General;
using ImageService.Api.Requests;
using ImageService.Infrastructure.Models.Flags;
using ImageService.Infrastructure.Models.ImageDTO;
using ImageService.Infrastructure.Repositories;
using MediatR;

namespace ImageService.Api.Handlers
{
    public class AddImageHandler : IRequestHandler<AddImage, ImageResponse>
    {
        IImageRepository _repository;
        ImageSettings _settings;
        public AddImageHandler(IImageRepository repository, ImageSettings settings)
        {
            _repository = repository;
            _settings = settings;
        }
        public async Task<ImageResponse> Handle(AddImage request, CancellationToken cancellationToken)
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

            string? uriSAS = null;

            using (MemoryStream image = new MemoryStream(Convert.FromBase64String(request.FileBase64)))
            {
                uriSAS = await _repository.AddByUrl(path, image);
            }

            if (uriSAS is null)
            {
                throw new ImageNotAddedException();
            }

            return new ImageResponse { Id = request.Id, Uri = uriSAS, Size = request.Size };
        }
    }
}
