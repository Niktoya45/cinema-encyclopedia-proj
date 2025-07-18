using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models;
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
using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.Flags;

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

        [BindProperty(SupportsGet=true)]
        public float UserScore { get; set; }

        public Label UserLabel { get; set; }

        public CinemaModel(IGatewayService gatewayService, UISettings settings) 
        {
            _gatewayService = gatewayService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet([FromRoute] string id, CancellationToken ct) 
        {
            if (TestEntities.Used)
            {
                Cinema = TestEntities.Cinema;
                UserScore = 5;
            }
                
            else
            {
                var response = await _gatewayService.GetCinemaById(id, ct);

                if (response is null)
                {
                    return RedirectToPage("/Index");
                }

                Cinema = new Cinema
                {
                    Id = response.Id,
                    Name = response.Name,
                    Picture = response.Picture,
                    PictureUri = response.PictureUri,
                    ReleaseDate = response.ReleaseDate,
                    Genres = response.Genres,
                    Language = response.Language,
                    RatingScore = response.Rating.Score,
                    Description = response.Description,

                };

                Cinema.Starrings = response.Starrings.Select(s => new EncyclopediaService.Api.Models.Display.Starring
                {
                    Id = s.Id,
                    Name = s.Name,
                    Picture = s.Picture,
                    PictureUri = s.PictureUri,
                    Jobs = s.Jobs,
                    RoleName = s.RoleName,
                    RolePriority = s.RolePriority
                }).ToArray();

                Cinema.ProductionStudios = response.ProductionStudios.Select(s => new ProductionStudio
                {
                    Id = s.Id,
                    Name = s.Name,
                    Picture = s.Picture,
                    PictureUri = s.PictureUri
                }).ToArray();

                if (User.IsLoggedIn())
                {
                    string userId = User.GetId();

                    var responseScore = await _gatewayService.GetUserRatingFor(userId, id, ct);
                    var responseLabels = await _gatewayService.GetUserLabelFor(userId, id, ct);

                    Label setLabel = responseLabels is null || responseLabels.LabeledCinemas.Count == 0 ? Label.None : responseLabels.LabeledCinemas.FirstOrDefault().Label;

                    UserScore = responseScore is null ? 0.0f : (float)responseScore.Rating;
                    UserLabel =  setLabel;
                }
            }

            EditMain = new EditMainCinema { Id = Cinema.Id, Name = Cinema.Name, ReleaseDate = Cinema.ReleaseDate, Language = Cinema.Language, Genres = Cinema.Genres, Description = Cinema.Description };

            EditPoster = new EditImage { ImageId=Cinema.Picture, ImageUri = Cinema.PictureUri };

            EditStarring = new EditStarring { };

            EditStudio = new EditStudio { };

            return Page();
        }

        public async Task<IActionResult> OnPostEditCinema([FromRoute] string id, CancellationToken ct)
        {
            ModelState.Remove("JobsBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditMain(true);
            }

            if (EditMain != null && !TestEntities.Used) 
            {
                var response = await _gatewayService.UpdateCinemaMain(id, new UpdateCinemaRequest
                {
                    Name = EditMain.Name,
                    ReleaseDate = EditMain.ReleaseDate,
                    Genres = EditMain.GenresBind is null ? EditMain.Genres : EditMain.GenresBind.Aggregate((acc, g) => acc | g),
                    Language = EditMain.Language.GetValueOrDefault(),
                    Description = EditMain.Description,
                }, ct); 
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteCinema([FromRoute] string id, CancellationToken ct) 
        {
            if (!TestEntities.Used)
            {
                bool response = await _gatewayService.DeleteCinema(id, ct);
            }

            return new OkObjectResult(id);
        }

        public async Task<IActionResult> OnPostRate([FromRoute] string id, [FromForm] byte score, CancellationToken ct)
        {
            if(!TestEntities.Used)
            {
                if (!User.IsLoggedIn())
                    return new OkObjectResult(0);

                string userId = User.GetId();

                var response = await _gatewayService.UpdateRatingList(userId, new UpdateRatingRequest {
                    Id = id,
                    Rating = score,
                    OldRating = UserScore // add value to form
                }, ct);
            }

            return new OkObjectResult(score);
        }

        public async Task<IActionResult> OnPostLabel([FromRoute] string id, [FromForm] int label, CancellationToken ct)
        {
            bool remove = label < 0;

            if (remove)
            {
                UserLabel = (Label)(-label);
            }
            else {
                UserLabel = (Label)(label);
            }

            if (!TestEntities.Used)
            {
                if (!User.IsLoggedIn())
                    return new OkObjectResult(0);

                string userId = User.GetId();

                if (remove)
                {
                    var response = await _gatewayService.DeleteFromLabeledList(userId, id, UserLabel, ct);

                    return new OkObjectResult(new { label = (byte)response.Label });
                }
                else 
                {
                    var response = await _gatewayService.CreateForLabeledList(userId, new CreateLabeledRequest
                    {
                        CinemaId = id,
                        Label = UserLabel
                    }, ct);

                    return new OkObjectResult(new { label = (byte)response.Label });
                }
            }

            return new OkObjectResult(new { label = (byte)UserLabel });
        }

        public async Task<IActionResult> OnPostEditPicture([FromRoute] string id, CancellationToken ct)
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
                Id = EditPoster.ImageId??"", 
                NewId = HashName, Size = (ImageSize)31, 
                FileBase64 = HashImage },
                ct);

            if (response is null)
            {
                return new OkObjectResult(new { PictureUri = EditPoster.ImageUri });
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostAddProductionStudio([FromRoute] string id, CancellationToken ct)
        {
            // Implement: set ParentId and send NewProductionStudio to mediatre proxy 
            ClearExtra();

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddProductionStudio(true);
            }

            if (!TestEntities.Used && EditStudio != null && EditStudio.Id != null)
            {
                var response = await _gatewayService.CreateCinemaProductionStudioFor(id, new CreateProductionStudioRequest
                {
                    Id = EditStudio.Id,
                    Name = EditStudio.Name,
                    Picture = EditStudio.Picture
                }, ct);
            }

            return Partial("_StudioLogoCard", new ProductionStudio
            {
                NewRecord = true,
                ParentId = null,
                Id = EditStudio.Id,
                Name = EditStudio.Name,
                Picture = EditStudio.Picture,
                PictureUri = EditStudio.PictureUri
            });
        }

        public async Task<IActionResult> OnPostDeleteProductionStudio([FromRoute] string id, CancellationToken ct)
        {
            if (!TestEntities.Used && RecordId != null)
            {
                if (RecordId != null)
                { 
                    var response = await _gatewayService.DeleteCinemaProductionStudio(id, RecordId, ct);

                    if(response)
                        return new OkObjectResult(RecordId);
                }
                return new OkObjectResult(null);
            }

            return new OkObjectResult(RecordId);
        }

        public async Task<IActionResult> OnPostAddStarring([FromRoute] string id, CancellationToken ct)
        {
            ModelState.Remove("GenresBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseAddStarring(true);
            }

            if (!TestEntities.Used && EditStarring != null && EditStarring.Id != null)
            {
                var response = await _gatewayService.CreateCinemaStarringFor(id, new CreateStarringRequest
                {
                    Id = EditStarring.Id,
                    Name = EditStarring.Name,
                    Picture = EditStarring.Picture,
                    Jobs = EditStarring.JobsBind.Aggregate((acc, j) => acc | j),
                    RoleName = EditStarring.RoleName,
                    RolePriority = EditStarring.RolePriority.GetValueOrDefault()
                }, ct);
            }

            EditStarring.Jobs = EditStarring.JobsBind.Aggregate((acc, j) => acc | j);

            return Partial("_StarringCard", new EncyclopediaService.Api.Models.Display.Starring
            {
                NewRecord = true,
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

        public async Task<IActionResult> OnPostEditStarring([FromRoute] string id, CancellationToken ct)
        {
            ModelState.Remove("GenresBind");

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditStarring(true);
            }

            if (!TestEntities.Used && EditStarring != null && EditStarring.Id != null)
            {
                var response = await _gatewayService.UpdateCinemaStarring(id, EditStarring.Id, new UpdateStarringRequest
                {
                    Name = EditStarring.Name,
                    Picture = EditStarring.Picture,
                    Jobs = EditStarring.JobsBind.Aggregate((acc, j) => acc | j),
                    RoleName = EditStarring.RoleName,
                    RolePriority = EditStarring.RolePriority.GetValueOrDefault()
                }, ct);
            }

            EditStarring.Jobs = EditStarring.JobsBind.Aggregate((acc, j) => acc | j);

            return Partial("_StarringCard", new EncyclopediaService.Api.Models.Display.Starring
            {
                NewRecord = true,
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

        public async Task<IActionResult> OnPostDeleteStarring([FromRoute] string id, CancellationToken ct)
        {
            // Implement: send delete request with ParentId and Id to mediatre proxy
            if (!TestEntities.Used)
            {
                var response = await _gatewayService.DeleteCinemaStarring(id, RecordId, ct);

                if (!response)
                {
                    return new OkObjectResult(null);
                }
            }

            return new OkObjectResult(RecordId);
        }

        public async Task<IActionResult> OnPostSearchProductionStudio(
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
                    var response = await _gatewayService.GetStudiosByIds(new string[] { recordId }, ct, null);

                    return new OkObjectResult( response is null ? null : response.Response.FirstOrDefault());
                }
            }

        }

        public async Task<IActionResult> OnPostSearchStarring(
            CancellationToken ct,
            [FromRoute] string id, [FromForm] string? recordId, [FromForm] string? search)
        {
            // transfer data instead of below

            if (recordId == "" || recordId is null)
            {
                IEnumerable<SearchResponse>? response = new List<SearchResponse>();

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
                if (TestRecords.Used)
                    return new OkObjectResult(TestRecords.SearchRecord(search));
                else
                {
                    var response = await _gatewayService.GetPersonsByIds(new string[] { recordId }, ct, null);

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
