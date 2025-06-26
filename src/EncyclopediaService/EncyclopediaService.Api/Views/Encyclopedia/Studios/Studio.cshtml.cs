using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Test;
using EncyclopediaService.Api.Models.TestData;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.GatewayService;
using EncyclopediaService.Infrastructure.Services.ImageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Studios
{
    public class StudioModel : PageModel
    {
        private IGatewayService _gatewayService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? RecordId { get; set; }

        [BindProperty]
        public Studio? Studio { get; set; }

        [BindProperty]
        public EditMainStudio? EditMain { get; set; }

        [BindProperty]
        public EditFilm EditFilm { get; set; } = default!;

        [BindProperty]
        public EditImage? EditLogo { get; set; }

        public StudioModel(IGatewayService gatewayService, UISettings settings)
        {
            _gatewayService = gatewayService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet([FromRoute] string id)
        {

            Studio = TestEntities.Studio;

            EditMain = new EditMainStudio { Id = Studio.Id, Name = Studio.Name, FoundDate = Studio.FoundDate, Country = Studio.Country, PresidentName = Studio.PresidentName, Description = Studio.Description };

            EditLogo = new EditImage { ImageId = Studio.Picture, ImageUri = Studio.PictureUri };

            EditFilm = new EditFilm { };


            return Page();
        }

        public async Task<IActionResult> OnPostEditStudio([FromRoute] string id)
        {

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditMain(true);
            }

            if (EditMain != null)
            {
                EditMain.Id = id;
            }

            return new OkObjectResult(null);
        }

        public async Task<IActionResult> OnPostAddFilmography([FromRoute] string id)
        {
            // Implement: set ParentId and send post NewFilmography to mediatre proxy 

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddFilmography(true);
            }

            return Partial("_FilmCard", new FilmographyRecord
            {
                Id = EditFilm.Id,
                Name = EditFilm.Name,
                Year = EditFilm.Year,
                Picture = EditFilm.Picture,
                PictureUri = EditFilm.PictureUri
            });
        }

        public async Task<IActionResult> OnPostDeleteFilmography([FromRoute] string id)
        {
            // Implement: send delete request specifying ParentId and Id to mediatre proxy

            return new OkObjectResult(RecordId);
        }

        public async Task<IActionResult> OnPostEditPicture([FromRoute] string id, CancellationToken ct)
        {
            // Implement: after receiving image name send put request to mediatre proxy

            if (EditLogo.Image is null)
            {
                return new OkObjectResult(new { PictureUri = EditLogo.ImageUri });
            }

            if (EditLogo.Image.Length >= _settings.MaxFileLength)
            {
                return new OkObjectResult(new { PictureUri = EditLogo.ImageUri });
            }

            string imageName = EditLogo.Image.FileName;
            string imageExt = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;
            string HashImage = EditLogo.Image.OpenReadStream().ToBase64();

            var response = await _gatewayService.UpdateCinemaPhoto(id, new ReplaceImageRequest
            {
                Id = EditLogo.ImageId,
                NewId = HashName,
                Size = (ImageSize)31,
                FileBase64 = HashImage
            },
            ct);

            if (response is null)
            {
                return new OkObjectResult(new { PictureUri = EditLogo.ImageUri });
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostSearchFilmography(
            CancellationToken ct,
            [FromRoute] string id, [FromForm] string recordId, [FromForm] string search)
        {
            // transfer data instead of below

            if (recordId == "" || recordId is null)
            {
                IEnumerable<SearchResponse> response = new List<SearchResponse>();

                response = TestRecords.SearchList(search);

                return new OkObjectResult(response);
            }

            else
            {
                return new OkObjectResult(TestRecords.SearchRecord(search));
            }

        }

        public IActionResult OnPostReuseEditMain(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }
            return Partial("_EditMain", EditMain);

        }

        public IActionResult OnPostReuseAddFilmography(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }

            return Partial("_AddFilm", EditFilm);

        }
    }
}
