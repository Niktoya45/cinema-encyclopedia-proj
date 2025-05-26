using GatewayAPIService.Infrastructure.Extensions;
using GatewayAPIService.Infrastructure.Services.CinemaService;
using GatewayAPIService.Infrastructure.Services.ImageService;
using GatewayAPIService.Infrastructure.Services.PersonService;
using GatewayAPIService.Infrastructure.Services.StudioService;
using Microsoft.AspNetCore.Mvc;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;

namespace GatewayAPIService.Api.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : Controller
    {

        public readonly ICinemaService _cinemaService;
        public readonly IPersonService _personService;
        public readonly IStudioService _studioService;
        public readonly IImageService _imageService;

        public PersonsController(
            ICinemaService cinemaService,
            IPersonService personService,
            IStudioService studioService,
            IImageService imageService
            )
        {
            _cinemaService = cinemaService;
            _personService = personService;
            _studioService = studioService;
            _imageService = imageService;
        }

        /// <summary>
        /// Get all persons by optional sort and pagination criteria
        /// </summary>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All persons list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No person was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(Page<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<PersonsResponse>? response = await _personService.Get(ct, st, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (PersonsResponse person in response.Response)
            {
                if (person.Picture != null)
                {
                    person.PictureUri = await _imageService.GetImage(person.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<SearchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search(
            CancellationToken ct,
            [FromQuery] string search,
            [FromQuery] Pagination? pg = null
        )
        {
            IEnumerable<SearchResponse>? response = await _personService.GetBySearch(search, ct, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (SearchResponse person in response)
            {
                if (person.Picture != null)
                {
                    person.PictureUri = await _imageService.GetImage(person.Picture, ImageSize.Tiny);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all persons by country with optional sort and pagination criteria
        /// </summary>
        /// <param name="country">country of origin</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All persons list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No person was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("country/{country}")]
        [ProducesResponseType(typeof(Page<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Country(
            CancellationToken ct,
            [FromRoute] Country country,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<PersonsResponse>? response = await _personService.GetByCountry(country, ct, st, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (PersonsResponse person in response.Response)
            {
                if (person.Picture != null)
                {
                    person.PictureUri = await _imageService.GetImage(person.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all persons by jobs with optional sort and pagination criteria
        /// </summary>
        /// <param name="jobs">jobs of persons to search for</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All persons list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No person was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("jobs/{jobs}")]
        [ProducesResponseType(typeof(Page<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Jobs(
            CancellationToken ct,
            [FromRoute] Job jobs,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<PersonsResponse>? response = await _personService.GetByJobs(jobs, ct, st, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (PersonsResponse person in response.Response)
            {
                if (person.Picture != null)
                {
                    person.PictureUri = await _imageService.GetImage(person.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get person by its id
        /// </summary>
        /// <param name="id">requested person id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromRoute] string id
            )
        {
            PersonResponse? response = await _personService.GetById(id, ct);

            if (response is null)
            {
                return BadRequest();
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            if (response.Filmography != null)
            {
                foreach (FilmographyResponse filmography in response.Filmography)
                {
                    if (filmography.Picture != null)
                    {
                        filmography.PictureUri = await _imageService.GetImage(filmography.Picture, ImageSize.Small);
                    }
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Post new person made of request parameter
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Newly created person instance</returns>
        /// <response code="200">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(
            CancellationToken ct,
            [FromBody] CreatePersonRequest request
            )
        {
            PersonResponse? response = await _personService.Create(request, ct);

            if (response is null) { 
                return BadRequest(request);
            }

            return Ok(response);
        }

        /// <summary>
        /// Add cinema to person filmography made of request parameter
        /// </summary>
        /// <param name="personId">person id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created filmography instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpPost("{personId}/filmography/create")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Filmography(
            [FromRoute] string personId,
            [FromBody] CreateFilmographyRequest request,
            CancellationToken ct
            )
        {

            if (request.Name is null)             
            {
                CinemaResponse? cinema = await _cinemaService.GetById(request.Id, ct);

                if (cinema is null) 
                {
                    return BadRequest(request.Id);
                }

                request.Name = cinema.Name;
                request.Year = cinema.ReleaseDate.Year;
                request.Picture = cinema.Picture;
            }

            PersonResponse? person = await _personService.GetById(personId, ct);

            if (person is null)
            {
                return BadRequest(personId);
            }

            FilmographyResponse? response = null;

            if (person.Filmography != null)
            {
                response = person.Filmography.FirstOrDefault(f => f.Id == request.Id);

                if (response != null)
                    return Ok(response);
            }

            response = await _personService.CreateFilmographyFor(personId, request, ct);

            if (response is null)
            {
                return BadRequest(request.Id);
            }

            StarringResponse? responseBack = await _cinemaService.CreateStarringFor(
                                                    request.Id,
                                                    new CreateStarringRequest
                                                    {
                                                        Id = person.Id,
                                                        Name = person.Name,
                                                        Jobs = Job.None,
                                                        RoleName = null,
                                                        RolePriority = RolePriority.None,
                                                        Picture = person.Picture
                                                    },
                                                    CancellationToken.None);

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Small);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update single person with provided request parameters
        /// </summary>
        /// <param name="id">id of person to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated person instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            CancellationToken ct,
            [FromRoute] string id,
            [FromBody] UpdatePersonRequest request
            )
        {
            PersonResponse? response = await _personService.Update(id, request, ct);

            if (response is null)
            {
                return BadRequest(id);
            }

            UpdateStarringRequest? updateRecord = new UpdateStarringRequest { 
                Name = response.Name,
                Picture = response.Picture
            };

            StarringResponse? compareRecord = null;

            bool? starringCommonsEquals = null;

            if (response.Filmography != null)
            {
                foreach (FilmographyResponse filmography in response.Filmography)
                {
                    compareRecord = await _cinemaService.GetStarringById(filmography.Id, id, ct);

                    updateRecord.RolePriority = compareRecord.RolePriority.GetValueOrDefault();
                    updateRecord.RoleName = compareRecord.RoleName;

                    starringCommonsEquals = updateRecord.SameCommons(compareRecord);

                    if (!starringCommonsEquals.GetValueOrDefault())
                    {
                        await _cinemaService.UpdateStarring(
                            filmography.Id,
                            id,
                            updateRecord,
                            ct);
                    }

                    if (filmography.Picture != null)
                    {
                        filmography.PictureUri = await _imageService.GetImage(filmography.Picture, ImageSize.Small);
                    }
                }
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="personId">Id of person to update picture for</param>
        /// <param name="request">request body with picture</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpPut("{personId}/picture/update")]
        [ProducesResponseType(typeof(UpdatePictureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            CancellationToken ct,
            [FromRoute] string personId,
            [FromBody] ReplaceImageRequest request
            )
        {
            string? PictureId;

            if (request.Id is null)
            {
                PictureId = await _imageService.AddImage(request.NewId, request.FileBase64, request.Size);
            }
            else PictureId = await _imageService.ReplaceImage(request.Id, request.NewId, request.FileBase64, request.Size);

            if (PictureId == null)
            {
                return BadRequest(request);
            }

            UpdatePictureResponse? response = await _personService.UpdatePhoto(personId, new UpdatePictureRequest { Picture = PictureId }, ct);

            if (response is null)
            {
                return BadRequest(personId);
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(PictureId, ImageSize.Big);
            }

            return Ok(response);
        }


        /// <summary>
        /// Delete single person by its id
        /// </summary>
        /// <param name="id">Id of person to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            CancellationToken ct,
            [FromRoute] string id
            )
        {
            PersonResponse? person = await _personService.GetById(id, ct);

            if (person is null)
            {
                return BadRequest(id);
            }

            var deleted = await _personService.Delete(id, ct);

            if (!deleted)
            {
                return BadRequest(id);
            }

            if (person.Filmography != null)
            {
                await _cinemaService.DeleteStarring(null, id, ct);
            }

            if (person.Picture != null)
            {
                await _imageService.DeleteImage(person.Picture, (ImageSize)31);
            }

            return Ok();
        }

        /// <summary>
        /// Delete single filmography entrance by optional person id
        /// </summary>
        /// <param name="personId">Id of person which filmography cinema to be deleted</param>
        /// <param name="filmographyId">Id of filmography entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person or cinema is not found</response>
        [HttpDelete("{personId}/filmography/delete/{filmographyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmography(
            CancellationToken ct,
            [FromRoute] string personId,
            [FromRoute] string filmographyId
            )
        {

            PersonResponse? person = await _personService.GetById(personId, ct);

            if (person is null)
            {
                return BadRequest(personId);
            }

            var deleted = await _personService.DeleteFilmography(person.Id, filmographyId, ct);

            if (!deleted) 
            {
                return BadRequest(filmographyId);
            }

            await _cinemaService.DeleteStarring(filmographyId, person.Id, ct);

            return Ok();
        }
    }
}
