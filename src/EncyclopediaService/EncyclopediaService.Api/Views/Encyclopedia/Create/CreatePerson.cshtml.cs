using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Models.Add;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Test;
using EncyclopediaService.Api.Models.TestData;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.GatewayService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EncyclopediaService.Api.Views.Encyclopedia.Create
{
    public class CreatePersonModel : PageModel
    {
        IGatewayService _gatewayService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty]
        public AddPerson? Person { get; set; } = default!;

        [BindProperty]
        public EditImage? AddPicture { get; set; }

        [BindProperty]
        public EditFilm? AddFilm { get; set; } = default!;

        public string Error { get; set; }

        public CreatePersonModel(IGatewayService gatewayService, UISettings settings)
        {
            _gatewayService = gatewayService;

            _settings = settings;
        }

        public async Task<IActionResult> OnGet()
        {
            Person = new AddPerson { };

            AddPicture = new EditImage { };

            AddFilm = new EditFilm { };

            return Page();
        }

        public async Task<IActionResult> OnPostAddPerson(CancellationToken ct)
        {
            ModelState.Remove("Id");
            ModelState.Remove("Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Person.JobsBind != null)
                Person.Jobs = Person.JobsBind.Aggregate((acc, j) => acc | j);

            if (Person != null)
            {

                string Id = "2";

                if (!TestEntities.Used)
                {
                    var response = await _gatewayService.CreatePerson(new CreatePersonRequest
                    {
                        Name = Person.Name,
                        BirthDate = Person.BirthDate,
                        Jobs = Person.Jobs,
                        Country = Person.Country.GetValueOrDefault(),
                        Picture = Person.Picture,
                        Filmography = Person.Filmography is null ? null : Person.Filmography.Select(s => new CreateFilmographyRequest
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Year = s.Year,
                            Picture = s.Picture
                        }).ToArray(),
                        Description = Person.Description
                    }, ct);

                    if (response == null)
                    {
                        Error = "Something went wrong creating person page.";

                        return Page();
                    }

                    Id = response.Id;

                    if (AddPicture.Image != null && AddPicture.Image.Length < _settings.MaxFileLength)
                    {
                        string imageName = AddPicture.Image.FileName;
                        string imageExt = Path.GetExtension(imageName);

                        string HashName = imageName.SHA_1() + imageExt;
                        string HashImage = AddPicture.Image.OpenReadStream().ToBase64();

                        var responsePhoto = await _gatewayService.UpdatePersonPhoto(response.Id, new ReplaceImageRequest
                        {
                            Id = "0",
                            NewId = HashName,
                            Size = (ImageSize)31,
                            FileBase64 = HashImage
                        },
                            ct);
                    }
                }

                return RedirectToPage("/Encyclopedia/Persons/Person", new { id = Id });
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddFilm()
        {
            ModelState.Remove("JobsBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddFilm(true);
            }

            return Partial("_FilmCard", new FilmographyRecord
            {
                ParentId = nameof(Person) + "." + nameof(Person.Filmography),
                Id = AddFilm.Id,
                Name = AddFilm.Name,
                Year = AddFilm.Year,
                Picture = AddFilm.Picture,
                PictureUri = AddFilm.PictureUri
            });
        }

        public async Task<IActionResult> OnPostSearchFilm(
            CancellationToken ct,
            [FromRoute] string id, [FromForm] string recordId, [FromForm] string search)
        {
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
                    if (recordId is null)
                        return new OkObjectResult(null);

                    var response = await _gatewayService.GetCinemasByIds(new string[] { recordId }, ct, null);

                    return new OkObjectResult(response is null ? null : response.Response.FirstOrDefault());
                }
            }

        }

        public IActionResult OnPostReuseAddFilm(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }

            return Partial("_AddFilm", AddFilm);

        }
    }
}
