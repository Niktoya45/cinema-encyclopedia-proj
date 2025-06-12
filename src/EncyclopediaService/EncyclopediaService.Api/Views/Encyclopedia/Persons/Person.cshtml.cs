using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.ImageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.ImageService.Models.Flags;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons
{
    public class PersonModel:PageModel
    {
        private IImageService _imageService { get; init; }
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

        public PersonModel(IImageService imageService, UISettings settings)
        {
            _imageService = imageService;
            _settings = settings;
        }
        public async Task<IActionResult> OnGet([FromRoute] string id) 
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
                    new CinemaRecord { Id = "1", Name = "Cinema Title Long Long", Year=2000, Picture=null},
                    new CinemaRecord { Id = "2", Name = "Cinema Title Long", Year=2000, Picture=null},
                    new CinemaRecord { Id = "3", Name = "Cinema Title", Year=2000, Picture=null},
                    new CinemaRecord { Id = "4", Name = "Cinema Title", Year=2000, Picture=null}
                },
                Description = "Person description goes here. A prominent actor.."
            };

            if (Person.Description is null)
            {
                Person.Description = "";
            }

            EditMain = new EditMainPerson { Id = Person.Id, Name = Person.Name, BirthDate = Person.BirthDate, Country = Person.Country, Jobs = Person.Jobs, Description = Person.Description };

            EditPicture = new EditImage { ImageId = null, ImageUri=Person.Picture };

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

        public async Task<IActionResult> OnPostEditPicture([FromRoute] string id)
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

            string imageName = EditPicture.Image.FileName;
            string imageExt = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;

            if (EditPicture.ImageId is null || EditPicture.ImageId == String.Empty)
            {
                // if cinema yet has no image

                await _imageService.AddImage(HashName, EditPicture.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }
            else if (EditPicture.ImageId != HashName)
            {
                // if cinema already has an image

                await _imageService.ReplaceImage(EditPicture!.ImageId, HashName, EditPicture.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }

            return await OnGet(id);
        }
    }
}
