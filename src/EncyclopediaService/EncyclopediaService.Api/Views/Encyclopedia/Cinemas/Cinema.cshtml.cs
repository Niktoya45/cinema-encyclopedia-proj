using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.ImageService;
using EncyclopediaService.Api.Models.Display;
using Shared.CinemaDataService.Models.SharedDTO;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas
{
    public class CinemaModel:PageModel
    {
        private IImageService _imageService { get; init; }
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

        public CinemaModel(IImageService imageService, UISettings settings) 
        {
            _imageService = imageService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet(string id) 
        {
            string path = Request.Path;

            // send data request instead of block below

            Cinema = new Cinema
            {
                Id = id,
                Name = "Cinema Title With Additional Length for Test Purposes",
                Picture = null,
                PictureUri = null,
                ReleaseDate = new DateOnly(2004, 12, 4),
                Genres = Genre.Western | Genre.Mystery | Genre.Thriller,
                Language = 0,
                RatingScore = 5.5,
                ProductionStudios = new StudioRecord[] { new StudioRecord { Id = "1", Picture = "/img/grid_logo_placeholder.png" }, new StudioRecord { Id = "2", Picture = "/img/grid_logo_placeholder.png" } },
                Starrings = new Starring[] {
                    new Starring { Id = "1", Name="Name Surname 1", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Id = "2", Name="Name Surname 2", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Id = "3", Name="Name Surname 3", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Id = "4", Name="Name Surname 4", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Id = "5", Name="Name Surname 5", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                    new Starring { Id = "6", Name="Name Surname 6", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                    new Starring { Id = "7", Name="Name Surname 7", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                    new Starring { Id = "8", Name="Name Surname 8", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                },
                Description = "Cinema description goes here. This cinema is about.."
            };
            

            if (Cinema.Description is null) 
            {
                Cinema.Description = "";
            }

            EditMain = new EditMainCinema { Id = Cinema.Id, Name = Cinema.Name, ReleaseDate = Cinema.ReleaseDate, Language = Cinema.Language, Genres = Cinema.Genres, Description = Cinema.Description };

            EditPoster = new EditImage {ImageId=Cinema.Picture, ImageUri = Cinema.PictureUri};

            EditStarring = new EditStarring { Picture = _settings.DefaultSmallPersonPicture };

            EditStudio = new EditStudio { Id = "", Name = "", Picture = _settings.DefaultSmallLogoPicture };

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
                Cinema.Id = id;
                Cinema.Genres = EditMain.GenresBind.Aggregate((acc, g) => acc | g);

                //var response = _gateway.UpdateCinema(Cinema); 
                //return Ok(response);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditPoster([FromRoute] string id)
        {
            // Implement: after receiving image name send put request to mediatre proxy

            if (EditPoster.Image is null) 
            {
                // handle error 
                return await OnGet(id);
            }

            if (EditPoster.Image.Length >= _settings.MaxFileLength)
            {
                // handle error?
                return await OnGet(id);
            }

            string imageName = EditPoster.Image.FileName;
            string imageExt  = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;
             
            if (EditPoster.ImageId is null || EditPoster.ImageId == String.Empty)
            {
                // if cinema yet has no image

                await _imageService.AddImage(HashName, EditPoster.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }
            else if(EditPoster.ImageId != HashName) 
            {
                // if cinema already has an image

                await _imageService.ReplaceImage(EditPoster!.ImageId, HashName, EditPoster.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddProductionStudio([FromRoute] string id)
        {
            // Implement: set ParentId and send NewProductionStudio to mediatre proxy 
            ClearExtra();

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddProductionStudio(true);
            }

            return Partial("_StudioLogoCard", new StudioRecord
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

        public async Task OnPostRate([FromRoute] string id, [FromForm] byte score)
        {
            UserScore = score;

            //return Page();
        }

        public async Task OnPostLabel([FromRoute] string id, [FromForm] byte label)
        {

            //return Page();
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

        public IActionResult OnPostReuseEditMain(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }
            return Partial("_EditMain", EditMain);

        }

        public IActionResult OnPostReuseEditStarring(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }
            return Partial("_AddStarring", EditStarring);

        }

        public async Task<IActionResult> OnPostSearchProductionStudio(
            CancellationToken ct,
            [FromRoute] string id, [FromForm] string recordId, [FromForm] string search)
        {
            // transfer data instead of below

            if (recordId == "" || recordId is null)
            {
                IEnumerable<SearchResponse> response = new List<SearchResponse>();

                response = new List<SearchResponse> { 
                    new SearchResponse { Id = "12", Name = search + " Search Name 12"},
                    new SearchResponse { Id = "13", Name = search + " Search Name 13"},
                    new SearchResponse { Id = "14", Name = search + " Search Name 14"},
                    new SearchResponse { Id = "15", Name = search + " Search Name 15"},
                };

                return new OkObjectResult(response);
            }

            else
            {
                return new OkObjectResult(new SearchResponse { Id ="12", Name = "Search Name 12", Picture = _settings.DefaultSmallPersonPicture, PictureUri = _settings.DefaultSmallPersonPicture });
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

                response = new List<SearchResponse> {
                    new SearchResponse { Id = "12", Name = search + "Search Name 12"},
                    new SearchResponse { Id = "13", Name = search + "Search Name 13"},
                    new SearchResponse { Id = "14", Name = search + "Search Name 14"},
                    new SearchResponse { Id = "15", Name = search + "Search Name 15"},
                };

                return new OkObjectResult(response);
            }
            else
            {
                return new OkObjectResult(new SearchResponse { Id = "12", Name = "Search Name 12", Picture = _settings.DefaultSmallPersonPicture, PictureUri = _settings.DefaultSmallPersonPicture });
            }

        }

        private void ClearExtra()
        {
            ModelState.Remove("GenresBind"); 
            ModelState.Remove("JobsBind"); 
        }

    }
}
