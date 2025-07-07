using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Test;
using EncyclopediaService.Api.Models.TestData;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.GatewayService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.PersonDTO;
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
        public async Task<IActionResult> OnGet([FromRoute] string id, CancellationToken ct) 
        {
            if (TestEntities.Used)
            {
                Person = TestEntities.Person;
            }
            else
            {
                var response = await _gatewayService.GetPersonById(id, ct);

                if (response is null)
                {
                    return RedirectToPage("/Index");
                }

                Person = new Person
                {
                    Id = response.Id,
                    Name = response.Name,
                    Picture = response.Picture,
                    PictureUri = response.PictureUri,
                    BirthDate = response.BirthDate,
                    Jobs = response.Jobs,
                    Country = response.Country,
                    Description = response.Description,

                };

                Person.Filmography = response.Filmography.Select(s => new FilmographyRecord
                {
                    Id = s.Id,
                    Name = s.Name,
                    Year = s.Year,
                    Picture = s.Picture,
                    PictureUri = s.PictureUri
                }).ToArray();
            }

            EditMain = new EditMainPerson { Id = Person.Id, Name = Person.Name, BirthDate = Person.BirthDate, Country = Person.Country, Jobs = Person.Jobs, Description = Person.Description };

            EditPicture = new EditImage { ImageId = Person.Picture, ImageUri = Person.PictureUri };

            EditFilm = new EditFilm { };


            return Page();
        }

        public async Task<IActionResult> OnPostEditPerson([FromRoute] string id, CancellationToken ct)
        {

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditMain(true);
            }

            if (EditMain != null && !TestEntities.Used)
            {
                var response = await _gatewayService.UpdatePersonMain(id, new UpdatePersonRequest
                {
                    Name = EditMain.Name,
                    BirthDate = EditMain.BirthDate,
                    Jobs = EditMain.JobsBind.Aggregate((acc, g) => acc | g),
                    Country = EditMain.Country.GetValueOrDefault(),
                    Description = EditMain.Description,
                }, ct);
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeletePerson([FromRoute] string id, CancellationToken ct)
        {
            if (!TestEntities.Used)
            {
                bool response = await _gatewayService.DeletePerson(id, ct);
            }

            return new OkObjectResult(id);
        }

        public async Task<IActionResult> OnPostAddFilmography([FromRoute] string id, CancellationToken ct)
        {
            ModelState.Remove("JobsBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddFilmography(true);
            }

            if (!TestEntities.Used && EditFilm != null && EditFilm.Id != null)
            {
                var response = await _gatewayService.CreatePersonFilmographyFor(id, new Shared.CinemaDataService.Models.RecordDTO.CreateFilmographyRequest
                {
                    Id = EditFilm.Id,
                    Name = EditFilm.Name,
                    Picture = EditFilm.Picture
                }, ct);

                if (response is null)
                {
                    return new OkObjectResult(null);
                }
            }

            return Partial("_FilmCard", new FilmographyRecord {
                Id = EditFilm.Id,
                Name = EditFilm.Name,
                Year = EditFilm.Year,
                Picture = EditFilm.Picture,
                PictureUri = EditFilm.PictureUri
            });
        }

        public async Task<IActionResult> OnPostDeleteFilmography([FromRoute] string id, CancellationToken ct)
        {
            if (TestEntities.Used && RecordId != null)
            {
                var response = await _gatewayService.DeleteStudioFilmography(id, RecordId, ct);

                if (!response) return new OkObjectResult(null);
            }

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

            var response = await _gatewayService.UpdatePersonPhoto(id, new ReplaceImageRequest
            {
                Id = EditPicture.ImageId??"",
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

                if (TestRecords.Used)
                    response = TestRecords.SearchList(search);
                else
                {
                    response = await _gatewayService.GetCinemasBySearch(search, ct, new Pagination(0, 5));
                }

                return new OkObjectResult(response);
            }

            else
            {
                if(TestRecords.Used)
                    return new OkObjectResult(TestRecords.SearchRecord(search));
                else
                {
                    var response = await _gatewayService.GetCinemasByIds(new string[] { recordId }, ct, null);

                    return new OkObjectResult(response is null ? null : response.Response.FirstOrDefault());
                }
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
