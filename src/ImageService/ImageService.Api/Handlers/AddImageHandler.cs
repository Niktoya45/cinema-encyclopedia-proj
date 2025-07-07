using Azure.Core;
using ImageMagick;
using ImageService.Api.Exceptions;
using ImageService.Api.General;
using ImageService.Api.Requests;
using ImageService.Infrastructure.Models.Flags;
using ImageService.Infrastructure.Models.ImageDTO;
using ImageService.Infrastructure.Repositories;
using MediatR;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;


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
            Dictionary<ImageSize, string> urisSAS = new Dictionary<ImageSize, string>();

            string? uriSAS = null;

            {
                byte[] originalBytes = Convert.FromBase64String(request.FileBase64);

                string path = "";

                foreach (ImageSize size in Enum.GetValues<ImageSize>())
                {
                    if ((request.Size & size) == 0)
                    {
                        continue;
                    }

                    path = _settings.RootDirectory + _settings.Directory[size] + request.Id;

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

                        try
                        {
                            uriSAS = await _repository.AddByUrl(path, new MemoryStream(image.ToByteArray()));
                        }
                        catch (Exception ex)
                        {
                            throw new ImageNotAddedException($"Image {request.Id} was not added for size: {size}\nMessage: {ex.Message}");
                        }

                        if (uriSAS is null)
                        {
                            throw new ImageNotAddedException($"Image {request.Id} was not added for size: {size}");
                        }

                        urisSAS.Add(size, uriSAS);
                    }
                }
            }

            return new ImageResponse { Id = request.Id, Uris = urisSAS, Size = request.Size };
        }
    }
}
