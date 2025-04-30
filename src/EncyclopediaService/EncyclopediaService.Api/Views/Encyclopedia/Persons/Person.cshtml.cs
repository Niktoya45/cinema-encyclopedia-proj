using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons
{
    public class PersonModel:PageModel
    {
        private BlobContainerClient _containerClient { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet=true)]
        public int Id { get; set; }

        [BindProperty]
        public Person? Person { get; set; }

        [BindProperty]
        public CinemaRecord NewFilmography { get; set; } = default!;

        [BindProperty]
        public EditMainPerson? EditMain { get; set; }

        [BindProperty]
        public EditImage? EditPicture { get; set; }

        public PersonModel(BlobContainerClient containerClient, UISettings settings)
        {
            _containerClient = containerClient;
            _settings = settings;
        }
        public async Task<IActionResult> OnGet(string id) 
        {
            // send data request instead of block below

            Person = new Person
            {
                Id = id,
                Name = "Long Long Name Long Long Long Surname",
                BirthDate = new DateOnly(1978, 1, 12),
                Country = 0,
                Jobs = Job.Actor,
                Picture = "/img/person_placeholder.png",
                Filmography = new CinemaRecord[] {
                    new CinemaRecord { Id = "1", Name = "Cinema Title Long Long", Year=2000, Picture="/img/grid_poster_placeholder.png"},
                    new CinemaRecord { Id = "2", Name = "Cinema Title Long", Year=2000, Picture="/img/grid_poster_placeholder.png"},
                    new CinemaRecord { Id = "3", Name = "Cinema Title", Year=2000, Picture="/img/grid_poster_placeholder.png"},
                    new CinemaRecord { Id = "4", Name = "Cinema Title", Year=2000, Picture="/img/grid_poster_placeholder.png"}
                },
                Description = "Person description goes here. A prominent actor.."
            };

            if (Person.Picture is null)
            {
                Person.Picture = _settings.DefaultPersonPicture;
            }

            if (Person.Description is null)
            {
                Person.Description = "";
            }

            EditMain = new EditMainPerson { Id = Person.Id, Name = Person.Name, BirthDate = Person.BirthDate, Country = Person.Country, Jobs = Person.Jobs, Description = Person.Description };

            EditPicture = new EditImage { ImageCurrent = Person.Picture };

            NewFilmography = new CinemaRecord { ParentId = Person.Id, Id = "", Name = "", Picture = _settings.DefaultSmallPosterPicture };


            return Page();
        }

        public async Task<IActionResult> OnPostEditPerson([FromRoute] string id)
        {
            // Implement: convert EditCinema to Cinema and send put request to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (EditMain != null)
            {
                Person.Id = id;
                Person.Name = EditMain.Name.Trim();
                Person.Jobs = EditMain.JobsBind.Aggregate((acc, j) => acc | j);
                Person.Description = EditMain.Description == null ? null : EditMain.Description.Trim();
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostAddFilmography([FromRoute] string id)
        {
            // Implement: set ParentId and send post NewFilmography to mediatre proxy 

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostDeleteFilmography([FromRoute] string id)
        {
            // Implement: send delete request specifying ParentId and Id to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostEditPoster([FromRoute] string id)
        {
            // Implement: after receiving image name send put request to mediatre proxy

            if (Person != null)
            {
                Person.Id = id;
            }

            if (EditPicture.Image is null)
            {
                // handle error 
                return await OnGet(id);
            }

            if (EditPicture.Image.Length >= _settings.MaxFileLength)
            {
                // handle error?
                return await OnGet(id);
            }

            if (EditPicture.ImageCurrent != _settings.DefaultPersonPicture)
            {
                var delres = await _containerClient.DeleteBlobIfExistsAsync(_settings.RootDirectory + EditPicture.ImageCurrent);

                if (!delres.Value)
                {
                    // handle
                }
            }

            string newname = string.Empty;

            using (Stream image = EditPicture.Image.OpenReadStream())
            {
                string imageName = EditPicture.Image.FileName;
                string ext = Path.GetExtension(imageName);

                string hash = imageName.SHA_1();

                newname = _settings.RootDirectory + hash + ext;

                await _containerClient.UploadBlobAsync(newname, image);
            }

            var uri = _containerClient.GetBlobClient(newname).GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(2)).AbsolutePath;

            return await OnGet(id);
        }
    }
}
