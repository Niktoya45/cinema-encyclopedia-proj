using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EncyclopediaService.Api.Models;
using Azure.Storage.Blobs;
using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.ImageService;
using Shared.ImageService.Models.Flags;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas
{
    public class CinemaModel:PageModel
    {
        private IImageService _imageService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty]
        public Cinema? Cinema { get; set; } = default!;

        [BindProperty]
        public StudioRecord NewProductionStudio { get; set; } = default!;

        [BindProperty]
        public EditMainCinema? EditMain { get; set; }

        [BindProperty]
        public EditStarring? EditStarring { get; set; } = default!;

        [BindProperty]
        public EditImage? EditPoster { get; set; }

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
                Picture = "/img/poster_placeholder.png",
                ReleaseDate = new DateOnly(2004, 12, 4),
                Genres = Genre.Western | Genre.Mystery | Genre.Thriller,
                Language = 0,
                RatingScore = 5.5,
                ProductionStudios = new StudioRecord[] { new StudioRecord { Picture = "/img/grid_logo_placeholder.png" }, new StudioRecord { Picture = "/img/grid_logo_placeholder.png" } },
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
            


            if (Cinema.Picture is null) 
            {
                Cinema.Picture = _settings.DefaultPosterPicture;
            }

            if (Cinema.Description is null) 
            {
                Cinema.Description = "";
            }

            EditMain = new EditMainCinema { Id = Cinema.Id, Name = Cinema.Name, ReleaseDate = Cinema.ReleaseDate, Language = Cinema.Language, Genres = Cinema.Genres, Description = Cinema.Description };

            EditPoster = new EditImage { ImageCurrent = Cinema.Picture};

            EditStarring = new EditStarring { Picture = _settings.DefaultSmallPersonPicture };

            NewProductionStudio = new StudioRecord { ParentId = Cinema.Id, Id = "", Name = "", Picture = _settings.DefaultSmallLogoPicture };

            return Page();
        }
        public async Task<IActionResult> OnPostEditCinema([FromRoute] string id)
        {
            // Implement: convert EditCinema to Cinema and send put request to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (EditMain != null) 
            {
                Cinema.Id = id;
                Cinema.Name = EditMain.Name.Trim();
                Cinema.Genres = EditMain.GenresBind.Aggregate((acc, g) => acc | g);
                Cinema.Description = EditMain.Description == null? null : EditMain.Description.Trim();
            }

            return await OnGet(id);
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
             
            if (EditPoster.ImageCurrent == _settings.DefaultPosterPicture)
            {
                // if cinema yet has no image

                await _imageService.AddImage(EditPoster.Image.FileName, EditPoster.Image.OpenReadStream().ToBase64(), ImageSize.None);
            }
            else { 
                // if cinema already has an image
                

            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostAddProductionStudio([FromRoute] string id)
        {
            // Implement: set ParentId and send NewProductionStudio to mediatre proxy 

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostDeleteProductionStudio([FromRoute] string id)
        {
            // Implement: send delete request specifying ParentId and Id to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostAddStarring([FromRoute] string id)
        {
            // Implement: convert EditStarring object to Starring, set ParentId and send post request to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }
        public PartialViewResult OnPostReuseEditStarringPartial()
        {
            EditStarring!.JobsBind = new List<Job>();
            return Partial("_EditStarring", EditStarring);

        }

        public async Task<IActionResult> OnPostEditStarring([FromRoute] string id)
        {
            // Implement: convert EditStarring object to Starring, set ParentId and send put request to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostDeleteStarring([FromRoute] string id)
        {
            // Implement: send delete request with ParentId and Id to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

    }
}
