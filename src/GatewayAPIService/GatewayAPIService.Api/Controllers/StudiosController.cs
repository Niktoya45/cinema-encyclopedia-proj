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
using Shared.CinemaDataService.Models.StudioDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;
using System.Diagnostics.Metrics;

namespace GatewayAPIService.Api.Controllers
{
    [ApiController]
    [Route("api/studios")]
    public class StudiosController : Controller
    {

        public readonly ICinemaService _cinemaService;
        public readonly IPersonService _personService;
        public readonly IStudioService _studioService;
        public readonly IImageService _imageService;

        public StudiosController(
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
        /// Get all studios by optional sort and pagination criteria
        /// </summary>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(Page<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<StudiosResponse>? response = await _studioService.Get(ct, st, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (StudiosResponse studio in response.Response)
            {
                if (studio.Picture != null)
                {
                    studio.PictureUri = await _imageService.GetImage(studio.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Search by studio name
        /// </summary>
        /// <param name="search">string tokens to use</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">None instance is found</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<SearchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search(
            CancellationToken ct,
            [FromRoute] string search,
            [FromQuery] Pagination? pg = null
        )
        {
            IEnumerable<SearchResponse>? response = await _studioService.GetBySearch(search, ct, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (SearchResponse studio in response)
            {
                if (studio.Picture != null)
                {
                    studio.PictureUri = await _imageService.GetImage(studio.Picture, ImageSize.Tiny);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all studios by provided year with optional sort and pagination criteria
        /// </summary>
        /// <param name="year">year of studio foundation</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("year/{year:int}")]
        [ProducesResponseType(typeof(Page<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Year(
            CancellationToken ct,
            [FromRoute] int year,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<StudiosResponse>? response = await _studioService.GetByYear(year, ct, st, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (StudiosResponse studio in response.Response)
            {
                if (studio.Picture != null)
                {
                    studio.PictureUri = await _imageService.GetImage(studio.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all studios by provided country with optional sort and pagination criteria
        /// </summary>
        /// <param name="country">country of studio foundation</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("country/{country}")]
        [ProducesResponseType(typeof(Page<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Country(
            CancellationToken ct,
            [FromRoute] Country country,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<StudiosResponse>? response = await _studioService.GetByCountry(country, ct, st, pg);

            if (response is null)
            {
                return BadRequest();
            }

            foreach (StudiosResponse studio in response.Response)
            {
                if (studio.Picture != null)
                {
                    studio.PictureUri = await _imageService.GetImage(studio.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get studio by its id
        /// </summary>
        /// <param name="id">requested studio id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromRoute] string id
            )
        {
            StudioResponse? response = await _studioService.GetById(id, ct);

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
        /// Post new studio made of request parameter
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(
            CancellationToken ct,
            [FromBody] CreateStudioRequest request
            )
        {
            StudioResponse? response = await _studioService.Create(request, ct);

            if (response is null)
            {
                return BadRequest(request);
            }

            return Ok(response);
        }

        /// <summary>
        /// Add cinema to studio filmography made of request parameter
        /// </summary>
        /// <param name="studioId">studio id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{studioId}/filmography/create")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Filmography(
            CancellationToken ct,
            [FromRoute] string studioId,
            [FromBody] CreateFilmographyRequest request
            )
        {
            CinemaResponse? cinema = await _cinemaService.GetById(request.Id, ct);

            if (cinema is null)
            {
                return BadRequest(request.Id);
            }

            request.Name = cinema.Name;
            request.Year = cinema.ReleaseDate.Year;
            request.Picture = cinema.Picture;

            StudioResponse? studio = await _studioService.GetById(studioId, ct);

            if (studio is null)
            {
                return BadRequest(studioId);
            }

            FilmographyResponse? response = null;

            if (studio.Filmography != null)
            {
                response = studio.Filmography.FirstOrDefault(f => f.Id == request.Id);

                if (response != null)
                    return Ok(response);
            }

            response = await _studioService.CreateFilmographyFor(studioId, request, ct);

            if (response is null)
            {
                return BadRequest(request.Id);
            }

            ProductionStudioResponse? responseBack = await _cinemaService.CreateProductionStudioFor(
                                                    request.Id,
                                                    new CreateProductionStudioRequest
                                                    {
                                                        Id = studio.Id,
                                                        Name = studio.Name,
                                                        Picture = studio.Picture
                                                    },
                                                    CancellationToken.None);

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Small);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update single studio with provided request parameters
        /// </summary>
        /// <param name="id">id of studio to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] string id,
            [FromBody] UpdateStudioRequest request,
            CancellationToken ct
            )
        {
            var response = await _studioService.Update(id, request, ct);

            UpdateProductionStudioRequest? updateRecord = new UpdateProductionStudioRequest
            {
                Name = response.Name,
                Picture = response.Picture
            };

            ProductionStudioResponse? compareRecord = null;

            bool? studioCommonsEquals = null;


            if (response.Filmography != null)
            {
                compareRecord ??= await _cinemaService.GetProductionStudioById(response.Filmography.FirstOrDefault().Id, id, ct);

                studioCommonsEquals ??= updateRecord.SameCommons(compareRecord);

                foreach (FilmographyResponse filmography in response.Filmography)
                {

                    if (!studioCommonsEquals.GetValueOrDefault())
                    {
                        await _cinemaService.UpdateProductionStudio(
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

            return Ok(response);
        }


        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="id">id of studio to be updated</param>
        /// <param name="request">request body with picture</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPut("{studioId}/picture/update")]
        [ProducesResponseType(typeof(UpdatePictureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            CancellationToken ct,
            [FromRoute] string studioId,
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

            UpdatePictureResponse? response = await _studioService.UpdatePhoto(studioId, new UpdatePictureRequest { Picture = PictureId }, ct);

            if (response is null)
            {
                return BadRequest(studioId);
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(PictureId, ImageSize.Big);
            }

            return Ok(response);
        }

        /// <summary>
        /// Delete single studio by its name
        /// </summary>
        /// <param name="id">Id of studio to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            CancellationToken ct,
            [FromRoute] string id
            )
        {
            StudioResponse? studio = await _studioService.GetById(id, ct);

            if (studio is null)
            {
                return BadRequest(id);
            }

            var deleted = await _studioService.Delete(id, ct);

            if (!deleted)
            {
                return BadRequest(id);
            }

            if (studio.Filmography != null)
            {
                await _cinemaService.DeleteStarring(null, id, ct);
            }

            if (studio.Picture != null)
            {
                await _imageService.DeleteImage(studio.Picture, (ImageSize)31);
            }

            return Ok();
        }

        /// <summary>
        /// Delete single cinema from filmography by optional studio id
        /// </summary>
        /// <param name="studioId">Id of studio which filmography cinema to be deleted</param>
        /// <param name="id">Id of filmography entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio or cinema is not found</response>
        [HttpDelete("{studioId}/filmography/delete/{filmographyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmography(
            CancellationToken ct,
            [FromRoute] string studioId,
            [FromRoute] string filmographyId
            )
        {

            StudioResponse? studio = await _studioService.GetById(studioId, ct);

            if (studio is null)
            {
                return BadRequest(studioId);
            }

            var deleted = await _studioService.DeleteFilmography(studio.Id, filmographyId, ct);

            if (!deleted)
            {
                return BadRequest(filmographyId);
            }

            await _cinemaService.DeleteStarring(filmographyId, studio.Id, ct);

            return Ok();
        }
    }
}
