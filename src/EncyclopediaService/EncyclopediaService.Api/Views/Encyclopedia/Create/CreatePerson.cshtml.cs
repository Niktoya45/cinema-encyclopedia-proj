using EncyclopediaService.Api.Models.Add;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

            Person.Picture = _settings.DefaultPersonPicture;
            AddPicture.ImageId = _settings.DefaultPersonPicture;
            AddFilm.Picture = _settings.DefaultSmallPosterPicture;

            return Page();
        }

        public async Task<IActionResult> OnPostAddPerson()
        {
            return new OkObjectResult(Person);
        }

        public async Task<IActionResult> OnPostAddFilm()
        {
            return Partial("_FilmCard", new CinemaRecord
            {
                ParentId = nameof(Person) + "." + nameof(Person.Filmography),
                Id = "3",
                Name = AddFilm.Name,
                Year = AddFilm.Year,
                Picture = AddFilm.Picture,
            });
        }
    }
}
