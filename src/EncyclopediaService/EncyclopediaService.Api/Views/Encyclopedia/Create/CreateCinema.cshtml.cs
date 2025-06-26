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
    public class CreateCinemaModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }


        [BindProperty]
        public AddCinema Cinema { get; set; } = default!;

        [BindProperty]
        public EditImage? AddPoster { get; set; }

        [BindProperty]
        public EditStarring? AddStarring { get; set; } = default!;

        [BindProperty]
        public EditStudio? AddStudio { get; set; } = default!;


        public CreateCinemaModel() 
        {
        }

        public async Task<IActionResult> OnGet()
        {
            Cinema = new AddCinema {  };

            AddPoster = new EditImage { };

            AddStudio = new EditStudio { };

            AddStarring = new EditStarring { };

            return Page();
        }

        public async Task<IActionResult> OnPostAddCinema() 
        {

            ModelState.Remove("JobsBind");
            if (Cinema.Starrings != null)
            {
                foreach (var s in Cinema.Starrings)
                    ModelState.Remove($"Cinema.Starrings[{s.Id}].JobsBind");
            }
            ModelState.Remove("Name");

            if (Cinema.GenresBind != null)
                Cinema.Genres = Cinema.GenresBind.Aggregate((acc, g) => acc | g);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var response = _gateway.CreateCinema(Cinema); 
            //return Ok(response);
  

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddStarring() 
        {
            ModelState.Remove("GenresBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddStarring(true);
            }

            AddStarring.Jobs = AddStarring.JobsBind.Aggregate((acc, j) => acc | j);

            return Partial("_StarringCard", new Starring {
                    ParentId = nameof(Cinema) + "." + nameof(Cinema.Starrings),
                    Id = "3",
                    Name = AddStarring.Name,
                    Picture = AddStarring.Picture,
                    PictureUri = AddStarring.PictureUri,
                    Jobs = AddStarring.Jobs,
                    RoleName = AddStarring.RoleName,
                    RolePriority = AddStarring.RolePriority,
            });
        }

        public async Task<IActionResult> OnPostDeleteStarring([FromQuery] string recordId)
        {
            return new OkObjectResult(recordId);
        }

        public async Task<IActionResult> OnPostAddProductionStudio() 
        {
            ClearExtra();

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddProductionStudio(true);
            }

            return Partial("_StudioCard", new ProductionStudio
            {
                ParentId = nameof(Cinema) + "." + nameof(Cinema.ProductionStudios),
                Id = "7",
                Name = AddStudio.Name,
                Picture = AddStudio.Picture,
                PictureUri = AddStudio.PictureUri
            });
        }

        public async Task<IActionResult> OnPostDeleteProductionStudio([FromQuery] string recordId)
        {
            return new OkObjectResult(recordId);
        }

        public async Task<IActionResult> OnPostSearchProductionStudio(
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

        public async Task<IActionResult> OnPostSearchStarring(
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

        public IActionResult OnPostReuseAddProductionStudio(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }

            return Partial("_AddStudio", AddStudio);

        }

        public IActionResult OnPostReuseAddStarring(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }

            return Partial("_AddStarring", AddStarring);

        }

        private void ClearExtra()
        {
            ModelState.Remove("GenresBind");
            ModelState.Remove("JobsBind");
        }
    }
}
