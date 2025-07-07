using EncyclopediaService.Api.Extensions;
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
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Create
{
    public class CreateCinemaModel : PageModel
    {
        IGatewayService _gatewayService { get; init; }

        UISettings _settings { get; init; }


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

        public string Error { get; set; }

        public CreateCinemaModel(IGatewayService gatewayService, UISettings settings) 
        {
            _gatewayService = gatewayService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet()
        {
            Cinema = new AddCinema {  };

            AddPoster = new EditImage { };

            AddStudio = new EditStudio { };

            AddStarring = new EditStarring { };

            return Page();
        }

        public async Task<IActionResult> OnPostAddCinema(CancellationToken ct) 
        {

            ModelState.Remove("JobsBind");
            if (Cinema.Starrings != null)
            {
                foreach (var s in Cinema.Starrings)
                    ModelState.Remove($"Cinema.Starrings[{s.Id}].JobsBind");
            }
            ModelState.Remove("Id");
            ModelState.Remove("Name");

            if (Cinema.GenresBind != null)
                Cinema.Genres = Cinema.GenresBind.Aggregate((acc, g) => acc | g);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string Id = "2";

            if (!TestEntities.Used)
            {
                var response = await _gatewayService.CreateCinema(new CreateCinemaRequest
                {
                    Name = Cinema.Name,
                    ReleaseDate = Cinema.ReleaseDate,
                    Genres = Cinema.Genres,
                    Language = Cinema.Language.GetValueOrDefault(),
                    Picture = Cinema.Picture,
                    ProductionStudios = Cinema.ProductionStudios is null ? null : Cinema.ProductionStudios.Select(s => new CreateProductionStudioRequest
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Picture = s.Picture
                    }).ToArray(),
                    Starrings = Cinema.Starrings is null ? null : Cinema.Starrings.Select(s => new CreateStarringRequest
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Picture = s.Picture,
                        Jobs = s.JobsBind.Aggregate((acc, j) => acc | j),
                        RoleName = s.RoleName,
                        RolePriority = s.RolePriority.GetValueOrDefault()
                    }).ToArray(),
                    Description = Cinema.Description
                }, ct);

                if (response == null)
                {
                    Error = "Something went wrong creating cinema page.";

                    return Page();
                }

                Id = response.Id;

                if (AddPoster.Image != null && AddPoster.Image.Length < _settings.MaxFileLength)
                {
                    string imageName = AddPoster.Image.FileName;
                    string imageExt = Path.GetExtension(imageName);

                    string HashName = imageName.SHA_1() + imageExt;
                    string HashImage = AddPoster.Image.OpenReadStream().ToBase64();

                    var responsePhoto = await _gatewayService.UpdateCinemaPhoto(response.Id, new ReplaceImageRequest
                    {
                        Id = "0",
                        NewId = HashName,
                        Size = (ImageSize)31,
                        FileBase64 = HashImage
                    },
                        ct);
                }

            }

            return RedirectToPage("/Encyclopedia/Cinemas/Cinema", new { id = Id });
        }

        public async Task<IActionResult> OnPostAddStarring() 
        {
            ModelState.Remove("GenresBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddStarring(true);
            }

            AddStarring.Jobs = AddStarring.JobsBind.Aggregate((acc, j) => acc | j);

            return Partial("_StarringCard", new EncyclopediaService.Api.Models.Display.Starring {
                    ParentId = nameof(Cinema) + "." + nameof(Cinema.Starrings),
                    Id = AddStarring.Id,
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
                Id = AddStudio.Id,
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

                if (TestRecords.Used)
                    response = TestRecords.SearchList(search);
                else 
                {
                    response = await _gatewayService.GetStudiosBySearch(search, ct, new Pagination(0, 5));
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

                    var response = await _gatewayService.GetStudiosByIds(new string[] { recordId }, ct, null);

                    return new OkObjectResult(response is null ? null : response.Response.FirstOrDefault());
                }
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

                if (TestRecords.Used)
                    response = TestRecords.SearchList(search);
                else
                {
                    response = await _gatewayService.GetPersonsBySearch(search, ct, new Pagination(0, 5));
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

                    var response = await _gatewayService.GetPersonsByIds(new string[] { recordId }, ct,  null);

                    return new OkObjectResult(response is null ? null : response.Response.FirstOrDefault());
                }

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
