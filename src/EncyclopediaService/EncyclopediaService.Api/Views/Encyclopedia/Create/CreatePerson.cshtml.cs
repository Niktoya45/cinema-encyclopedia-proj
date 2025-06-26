using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Models.Add;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.TestData;
using EncyclopediaService.Api.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.SharedDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Create
{
    public class CreatePersonModel : PageModel
    {
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty]
        public AddPerson? Person { get; set; } = default!;

        [BindProperty]
        public EditImage? AddPicture { get; set; }

        [BindProperty]
        public EditFilm? AddFilm { get; set; } = default!;

        public CreatePersonModel(UISettings settings)
        {
            _settings = settings;
        }

        public async Task<IActionResult> OnGet()
        {
            Person = new AddPerson { };

            AddPicture = new EditImage { };

            AddFilm = new EditFilm { };

            return Page();
        }

        public async Task<IActionResult> OnPostAddPerson()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Person != null)
            {
                //var response = _gateway.CreateCinema(Cinema); 
                //return Ok(response);
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
                Id = "3",
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
