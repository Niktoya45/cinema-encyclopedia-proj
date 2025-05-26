using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Add;

namespace EncyclopediaService.Api.Views.Encyclopedia.Create
{
    public class CreateCinemaModel : PageModel
    {
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty]
        public AddCinema? Cinema { get; set; } = default!;

        [BindProperty]
        public EditImage? AddPoster { get; set; }

        [BindProperty]
        public EditStarring? AddStarring { get; set; } = default!;

        [BindProperty]
        public EditStudio AddStudio { get; set; } = default!;


        public CreateCinemaModel(UISettings settings) 
        {
            _settings = settings;
        }

        public async Task<IActionResult> OnGet()
        {
            Cinema = new AddCinema {  };

            AddPoster = new EditImage { };

            AddStudio = new EditStudio { };

            AddStarring = new EditStarring { };

            Cinema.Picture = _settings.DefaultPosterPicture;
            AddPoster.ImageId = _settings.DefaultPosterPicture;
            AddStudio.Picture = _settings.DefaultSmallLogoPicture;
            AddStarring.Picture = _settings.DefaultSmallPersonPicture;

            return Page();
        }

        public async Task<IActionResult> OnPostAddCinema() 
        {
            return new OkObjectResult(Cinema);
        }

        public async Task<IActionResult> OnPostAddStarring() 
        {
            return Partial("_StarringCard", new Starring {
                    ParentId = nameof(Cinema) + "." + nameof(Cinema.Starrings),
                    Id = "3",
                    Name = AddStarring.Name,
                    Picture = AddStarring.Picture,
                    Jobs = AddStarring.Jobs,
                    RoleName = AddStarring.RoleName,
                    RolePriority = AddStarring.RolePriority,
            });
        }

        public async Task<IActionResult> OnPostAddStudio() 
        {
            return Partial("_StudioCard", new StudioRecord
            {
                ParentId = nameof(Cinema) + "." + nameof(Cinema.ProductionStudios),
                Id = "3",
                Name = AddStudio.Name,
                Picture = AddStudio.Picture
            });
        }
    }
}
