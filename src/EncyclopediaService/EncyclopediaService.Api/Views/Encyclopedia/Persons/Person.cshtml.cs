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
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons
{
    public class PersonModel:PageModel
    {
        private IGatewayService _gatewayService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet=true)]
        public string Id { get; set; }

        [BindProperty(SupportsGet=true)]
        public string? RecordId { get; set; }

        [BindProperty]
        public Person? Person { get; set; }

        [BindProperty]
        public EditMainPerson? EditMain { get; set; }

        [BindProperty]
        public EditFilm EditFilm { get; set; } = default!;

        [BindProperty]
        public EditImage? EditPicture { get; set; }

        public PersonModel(IGatewayService gatewayService, UISettings settings)
        {
            _gatewayService = gatewayService;
            _settings = settings;
        }
        public async Task<IActionResult> OnGet([FromRoute] string id) 
        {
            // send data request instead of block below

            Person = TestEntities.Person;

            EditMain = new EditMainPerson { Id = Person.Id, Name = Person.Name, BirthDate = Person.BirthDate, Country = Person.Country, Jobs = Person.Jobs, Description = Person.Description };

            EditPicture = new EditImage { ImageId = Person.Picture, ImageUri = Person.PictureUri };

            EditFilm = new EditFilm { };


            return Page();
        }

        public async Task<IActionResult> OnPostEditPerson([FromRoute] string id)
        {

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditMain(true);
            }

            if (EditMain != null)
            {
                EditMain.Id = id;
                EditMain.Jobs = EditMain.JobsBind.Aggregate((acc, j) => acc | j);
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostAddFilmography([FromRoute] string id)
        {
            // Implement: set ParentId and send post NewFilmography to mediatre proxy 

            ModelState.Remove("JobsBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddFilmography(true);
            }

            return Partial("_FilmCard", new FilmographyRecord {
                Id = EditFilm.Id,
                Name = EditFilm.Name,
                Year = 2001,
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

            if (EditPicture.Image is null)
            {
                return new OkObjectResult(new { PictureUri = EditPicture.ImageUri });
            }

            if (EditPicture.Image.Length >= _settings.MaxFileLength)
            {
                return new OkObjectResult(new { PictureUri = EditPicture.ImageUri });
            }

            string imageName = EditPicture.Image.FileName;
            string imageExt = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;
            string HashImage = EditPicture.Image.OpenReadStream().ToBase64();

            var response = await _gatewayService.UpdateCinemaPhoto(id, new ReplaceImageRequest
            {
                Id = EditPicture.ImageId,
                NewId = HashName,
                Size = (ImageSize)31,
                FileBase64 = HashImage
            },
            ct);

            if (response is null)
            {
                return new OkObjectResult(new { PictureUri = EditPicture.ImageUri });
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
