using ImageMagick;
using ImageService.Api.Exceptions;
using ImageService.Api.General;
using ImageService.Api.Requests;
using ImageService.Infrastructure.Models.Flags;
using ImageService.Infrastructure.Models.ImageDTO;
using ImageService.Infrastructure.Repositories;
using MediatR;
using System.IO;

namespace ImageService.Api.Handlers
{
    public class ReplaceImageHandler : IRequestHandler<ReplaceImage, ImageResponse>
    {
        IImageRepository _repository;
        ImageSettings _settings;
        public ReplaceImageHandler(IImageRepository repository, ImageSettings settings)
        {
            _repository = repository;
            _settings = settings;
        }
        public async Task<ImageResponse> Handle(ReplaceImage request, CancellationToken cancellationToken)
        {

            Dictionary<ImageSize, string> urisSAS = new Dictionary<ImageSize, string>();

            string? uriSAS = null;

            {
                byte[] originalBytes = Convert.FromBase64String(request.FileBase64);

                string pathOld = "";

                string pathNew = "";


                foreach (ImageSize size in Enum.GetValues<ImageSize>())
                {
                    pathOld = _settings.RootDirectory + _settings.Directory[request.Size] + request.Id;

                    if ((request.Size & size) == 0)
                    {
                        await _repository.DeleteByUrl(pathOld);

                        continue;
                    }

                    pathNew = _settings.RootDirectory + _settings.Directory[request.Size] + request.NewId;

                    using (MagickImage image = new MagickImage(originalBytes))
                    {
                        if (image.Width < image.Height)
                        {
                            image.InterpolativeResize(_settings.Normalize[size], 0, PixelInterpolateMethod.Bilinear);
                        }
                        else
                        {
                            image.InterpolativeResize(0, _settings.Normalize[size], PixelInterpolateMethod.Bilinear);
                        }

                        uriSAS = await _repository.ReplaceByUrl(pathOld, pathNew, new MemoryStream(image.ToByteArray()));

                        if (uriSAS is null)
                        {
                            throw new ImageNotAddedException($"Image {request.Id} was not replaced with {request.NewId} for size: {size}");
                        }

                        urisSAS.Add(size, uriSAS);
                    }
                }
            }

            return new ImageResponse { Id = request.Id, Uris = urisSAS, Size = request.Size };

        }
    }
}
