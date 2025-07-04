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
                return NotFound();
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
        /// Get all studios by indexes and optional sort criteria
        /// </summary>
        /// <param name="ids">index list</param>
        /// <param name="st">sort parameters (optional)</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpPost("indexes")]
        [ProducesResponseType(typeof(Page<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromBody] string[] ids,
            [FromQuery] SortBy? st = null
            )
        {
            Page<StudiosResponse>? response = await _studioService.GetByIds(ids, ct, st);

            if (response is null)
            {
                return NotFound();
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
        [HttpGet("search/{search}")]
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
                return NotFound();
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
        /// Search by studio name
        /// </summary>
        /// <param name="search">string tokens to use</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">None instance is found</response>
        [HttpGet("search-page/{search}")]
        [ProducesResponseType(typeof(Page<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchPage(
            CancellationToken ct,
            [FromRoute] string search,
            [FromQuery] Pagination? pg = null
        )
        {
            Page<StudiosResponse>? response = await _studioService.GetBySearchPage(search, ct, pg);

            if (response is null)
            {
                return NotFound();
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
                return NotFound();
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
        /// Get all studios by provided year with optional sort and pagination criteria
        /// </summary>
        /// <param name="lower">year span lower boundary (>=)</param>
        /// <param name="upper">year span upper boundary (<)</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("year")]
        [ProducesResponseType(typeof(Page<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> YearSpan(
            CancellationToken ct,
            [FromQuery] int[] lys,
            [FromQuery] int d,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<StudiosResponse>? response = await _studioService.GetByYearSpans(lys, d, ct, st, pg);

            if (response is null)
            {
                return NotFound();
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
                return NotFound();
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
                return NotFound();
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
            Page<CinemasResponse>? responseFilm;
            CinemasResponse? filmRecord;

            if (request.Filmography != null && request.Filmography.Any())
            {
                foreach (var film in request.Filmography)
                {
                    responseFilm = await _cinemaService.GetByIds(new string[] { film.Id }, ct, null);


                    if (responseFilm == null) continue;

                    filmRecord = responseFilm.Response.FirstOrDefault();

                    if (filmRecord == null) continue;

                    film.Name = filmRecord.Name;
                    film.Year = filmRecord.Year;
                    film.Picture = filmRecord.Picture;
                }
            }

            StudioResponse? response = await _studioService.Create(request, ct);

            if (response is null)
            {
                return BadRequest(request);
            }

            if (request.Filmography != null && request.Filmography.Any())
            {
                foreach (var film in request.Filmography)
                {
                    await _cinemaService.CreateProductionStudioFor(film.Id, new CreateProductionStudioRequest
                    {
                        Id = response.Id,
                        Name = response.Name,
                        Picture = response.Picture
                    }, ct);
                }
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
                return NotFound(request.Id);
            }

            request.Name = cinema.Name;
            request.Year = cinema.ReleaseDate.Year;
            request.Picture = cinema.Picture;

            StudioResponse? studio = await _studioService.GetById(studioId, ct);

            if (studio is null)
            {
                return NotFound(studioId);
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
                return NotFound(request.Id);
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

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            if (response is null)
            {
                return NotFound();
            }

            await UpdateAdditional(response, ct);

            return Ok(response);
        }

        /// <summary>
        /// Update studio's main information with provided request parameters
        /// </summary>
        /// <param name="id">id of studio to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPut("{id}/main")]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMainAsync(
            [FromRoute] string id,
            [FromBody] UpdateStudioRequest request,
            CancellationToken ct
            )
        {
            var response = await _studioService.UpdateMain(id, request, ct);

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            if (response is null)
            {
                return NotFound();
            }

            await UpdateAdditional(response, ct);

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
            string? pictureUri;

            var response = await _studioService.GetById(studioId, ct);

            if (response is null)
            {
                return NotFound();
            }

            if (response.Picture is null)
            {
                pictureUri = await _imageService.AddImage(request.NewId, request.FileBase64, request.Size, ImageSize.Big);
            }
            else pictureUri = await _imageService.ReplaceImage(request.Id, request.NewId, request.FileBase64, request.Size, ImageSize.Big);

            if (pictureUri == null)
            {
                return NotFound(request);
            }

            UpdatePictureResponse? responsePhoto = await _studioService.UpdatePhoto(studioId, new UpdatePictureRequest { 
                Picture = response.Picture
            }, ct);
            if (responsePhoto is null)
            {
                return NotFound(studioId);
            }
            if (responsePhoto.Picture != null)
            {
                responsePhoto.PictureUri = pictureUri;
            }

            await UpdateAdditional(response, ct);

            return Ok(responsePhoto);
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
                return NotFound(id);
            }

            var deleted = await _studioService.Delete(id, ct);

            if (!deleted)
            {
                return NotFound(id);
            }

            if (studio.Filmography != null)
            {
                await _cinemaService.DeleteStarring(null, id, ct);
            }

            if (studio.Picture != null)
            {
                await _imageService.DeleteImage(studio.Picture, (ImageSize)31);
            }

            return Ok(id);
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
                return NotFound(studioId);
            }

            var deleted = await _studioService.DeleteFilmography(studio.Id, filmographyId, ct);

            if (!deleted)
            {
                return NotFound(filmographyId);
            }

            await _cinemaService.DeleteStarring(filmographyId, studio.Id, ct);

            return Ok(filmographyId);
        }

        protected async Task UpdateAdditional(StudioResponse studio, CancellationToken ct)
        {

            UpdateProductionStudioRequest? updateRecord = new UpdateProductionStudioRequest
            {
                Name = studio.Name,
                Picture = studio.Picture
            };

            ProductionStudioResponse? compareRecord = null;

            bool? studioCommonsEquals = null;


            if (studio.Filmography != null)
            {
                compareRecord ??= await _cinemaService.GetProductionStudioById(studio.Filmography.FirstOrDefault().Id, studio.Id, ct);

                studioCommonsEquals ??= updateRecord.SameCommons(compareRecord);

                foreach (FilmographyResponse filmography in studio.Filmography)
                {

                    if (!studioCommonsEquals.GetValueOrDefault())
                    {
                        await _cinemaService.UpdateProductionStudio(
                            filmography.Id,
                            studio.Id,
                            updateRecord,
                            ct);
                    }
                }
            }
        }
    }
}
