using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EncyclopediaService.Api.Models;
using Azure.Storage.Blobs;
using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas
{
    public class CinemaModel:PageModel
    {

        private BlobContainerClient _containerClient { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        [BindProperty]
        public Cinema? Cinema { get; set; } = default!;

        [BindProperty]
        public StudioRecord NewStudioRecord { get; set; } = default!;

        [BindProperty]
        public Starring NewStarring { get; set; } = default!;

        [BindProperty]
        public EditMainCinema? EditMain { get; set; }

        [BindProperty]
        public EditImage? EditPoster { get; set; }

        public CinemaModel(BlobContainerClient containerClient, UISettings settings) 
        {
            _containerClient = containerClient;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet(string id) 
        {
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
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Actor, RoleName="Role Name Long Long", RolePriority = RolePriority.Main},
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                    new Starring { Name="Name Surname", Picture="/img/grid_person_placeholder.png", Jobs=Job.Director, RoleName=null},
                },
                Description = "Cinema description goes here. This cinema is about.."
            };
            


            if (Cinema.Picture is null) 
            {
                Cinema.Picture = "/img/poster_placeholder.png";
            }

            if (Cinema.Description is null) 
            {
                Cinema.Description = "";
            }

            EditMain = new EditMainCinema { Id = Cinema.Id, Name = Cinema.Name, ReleaseDate = Cinema.ReleaseDate, Language = Cinema.Language, Genres = Cinema.Genres, Description = Cinema.Description };

            EditPoster = new EditImage { ImageCurrent = Cinema.Picture};

            return Page();
        }

        public async Task<IActionResult> OnPostEditCinema([FromRoute] string id)
        {
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
            if (Cinema != null)
            {
                Cinema.Id = id;
            }

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

            if (EditPoster.ImageCurrent != _settings.DefaultPosterPicture) 
            {
                var delres = await _containerClient.DeleteBlobIfExistsAsync(_settings.RootDirectory + EditPoster.ImageCurrent);

                if (!delres.Value) 
                {
                    // handle
                }
            }

            using (Stream image = EditPoster.Image.OpenReadStream())
            {
                string imageName = EditPoster.Image.FileName;
                string ext = Path.GetExtension(imageName);

                string hash = imageName.SHA_1();

                await _containerClient.UploadBlobAsync(_settings.RootDirectory+hash+ext, image);
            }
            
            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostAddProductionStudio([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (Cinema != null)
            {
                Cinema.Id = id;
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostEditProductionStudio([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (Cinema != null)
            {
                Cinema.Id = id;
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostDeleteProductionStudio([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (Cinema != null)
            {
                Cinema.Id = id;
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostAddStarring([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (Cinema != null)
            {
                Cinema.Id = id;
            }

            return await OnGet(id);
        }
        public async Task<IActionResult> OnPostEditStarring([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (Cinema != null)
            {
                Cinema.Id = id;
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostDeleteStarring([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (Cinema != null)
            {
                Cinema.Id = id;
            }

            return await OnGet(id);
        }

    }
}
