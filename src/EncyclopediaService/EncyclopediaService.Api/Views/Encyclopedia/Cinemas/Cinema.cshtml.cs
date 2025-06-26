using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.GatewayService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;
using Shared.UserDataService.Models.Flags;
using EncyclopediaService.Api.Models.Test;
using EncyclopediaService.Api.Models.TestData;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas
{
    public class CinemaModel:PageModel
    {
        private IGatewayService _gatewayService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? RecordId { get; set; }

        [BindProperty]
        public Cinema? Cinema { get; set; } = default!;

        [BindProperty]
        public EditMainCinema? EditMain { get; set; }

        [BindProperty]
        public EditStarring? EditStarring { get; set; } = default!;

        [BindProperty]
        public EditStudio? EditStudio { get; set; } = default!;

        [BindProperty]
        public EditImage? EditPoster { get; set; }

        public float UserScore { get; set; }

        public Label UserLabel { get; set; }

        public CinemaModel(IGatewayService gatewayService, UISettings settings) 
        {
            _gatewayService = gatewayService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet(string id) 
        {
            string path = Request.Path;

            // send data request instead of block below

            Cinema = TestEntities.Cinema;

            EditMain = new EditMainCinema { Id = Cinema.Id, Name = Cinema.Name, ReleaseDate = Cinema.ReleaseDate, Language = Cinema.Language, Genres = Cinema.Genres, Description = Cinema.Description };

            EditPoster = new EditImage { ImageId=Cinema.Picture, ImageUri = Cinema.PictureUri };

            EditStarring = new EditStarring { };

            EditStudio = new EditStudio { };

            return Page();
        }

        public async Task<IActionResult> OnPostEditCinema([FromRoute] string id)
        {
            // Implement: convert EditCinema to Cinema and send put request to mediatre proxy
            ModelState.Remove("JobsBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditMain(true);
            }

            if (EditMain != null) 
            {
                //var response = _gateway.UpdateCinema(Cinema); 
                //return Ok(response);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCinema([FromRoute] string id) 
        {
            return new OkObjectResult(id);
        }

        public async Task<IActionResult> OnPostRate([FromRoute] string id, [FromForm] byte score)
        {
            UserScore = score;

            return new OkObjectResult(UserScore);
        }

        public async Task<IActionResult> OnPostLabel([FromRoute] string id, [FromForm] int label)
        {

            if (label < 0)
            {
                UserLabel = (Label)(-label);
            }
            else {
                UserLabel = (Label)(label);
            }
            
            return new OkObjectResult((byte)UserLabel);
        }

        public async Task<IActionResult> OnPostEditPoster([FromRoute] string id, CancellationToken ct)
        {
            // Implement: after receiving image name send put request to mediatre proxy

            if (EditPoster.Image is null) 
            {
                return new OkObjectResult(new { PictureUri = EditPoster.ImageUri });
            }

            if (EditPoster.Image.Length >= _settings.MaxFileLength)
            {
                return new OkObjectResult(new { PictureUri = EditPoster.ImageUri });
            }

            string imageName = EditPoster.Image.FileName;
            string imageExt  = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;
            string HashImage = EditPoster.Image.OpenReadStream().ToBase64();

            var response = await _gatewayService.UpdateCinemaPhoto(id, new ReplaceImageRequest { 
                Id = EditPoster.ImageId, 
                NewId = HashName, Size = (ImageSize)31, 
                FileBase64 = HashImage },
                ct);

            if (response is null)
            {
                return new OkObjectResult(new { PictureUri = EditPoster.ImageUri });
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostAddProductionStudio([FromRoute] string id)
        {
            // Implement: set ParentId and send NewProductionStudio to mediatre proxy 
            ClearExtra();

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddProductionStudio(true);
            }

            return Partial("_StudioLogoCard", new ProductionStudio
            {
                ParentId = null,
                Id = EditStarring.Id,
                Name = EditStarring.Name,
                Picture = EditStarring.Picture,
                PictureUri = EditStarring.PictureUri
            });
        }

        public async Task<IActionResult> OnPostDeleteProductionStudio([FromRoute] string id)
        {
            // Implement: send delete request specifying ParentId and Id to mediatre proxy

            return new OkObjectResult(RecordId);
        }

        public async Task<IActionResult> OnPostAddStarring([FromRoute] string id)
        {
            // Implement: convert EditStarring object to Starring, set ParentId and send post request to mediatre proxy

            ModelState.Remove("GenresBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddStarring(true);
            }

            EditStarring.Jobs = EditStarring.JobsBind.Aggregate((acc, j) => acc | j);

            return Partial("_StarringCard", new Starring
            {
                ParentId = null,
                Id = EditStarring.Id,
                Name = EditStarring.Name,
                Picture = EditStarring.Picture,
                PictureUri = EditStarring.PictureUri,
                Jobs = EditStarring.Jobs,
                RoleName = EditStarring.RoleName,
                RolePriority = EditStarring.RolePriority
            });
        }

        public async Task<IActionResult> OnPostEditStarring([FromRoute] string id)
        {
            // Implement: convert EditStarring object to Starring, set ParentId and send put request to mediatre proxy

            ModelState.Remove("GenresBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditStarring(true);
            }

            EditStarring.Jobs = EditStarring.JobsBind.Aggregate((acc, j) => acc | j);

            return Partial("_StarringCard", new Starring
            {
                ParentId = null,
                Id = EditStarring.Id,
                Name = EditStarring.Name,
                Picture = EditStarring.Picture,
                PictureUri = EditStarring.PictureUri,
                Jobs = EditStarring.Jobs,
                RoleName = EditStarring.RoleName,
                RolePriority = EditStarring.RolePriority
            });
        }

        public async Task<IActionResult> OnPostDeleteStarring([FromRoute] string id)
        {
            // Implement: send delete request with ParentId and Id to mediatre proxy

            return new OkObjectResult(RecordId);
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

        public IActionResult OnPostReuseEditMain(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }
            return Partial("_EditMain", EditMain);

        }

        public IActionResult OnPostReuseAddProductionStudio(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }

            return Partial("_AddStudio", EditStudio);

        }

        public IActionResult OnPostReuseAddStarring(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }

            return Partial("_AddStarring", EditStarring);

        }

        public IActionResult OnPostReuseEditStarring(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }
            return Partial("_AddStarring", EditStarring);

        }

        private void ClearExtra()
        {
            ModelState.Remove("GenresBind"); 
            ModelState.Remove("JobsBind"); 
        }

    }
}
