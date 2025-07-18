using GatewayAPIService.Infrastructure.Extensions;
using GatewayAPIService.Infrastructure.Services.CinemaService;
using GatewayAPIService.Infrastructure.Services.ImageService;
using GatewayAPIService.Infrastructure.Services.PersonService;
using GatewayAPIService.Infrastructure.Services.StudioService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.CinemaDataService.Models.StudioDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;

namespace GatewayAPIService.Api.Controllers
{
    [ApiController]
    [Route("api/cinemas")]
    public class CinemasController : Controller
    {
        public readonly ICinemaService _cinemaService;
        public readonly IPersonService _personService;
        public readonly IStudioService _studioService;
        public readonly IImageService  _imageService;

        public CinemasController(
            ICinemaService cinemaService,
            IPersonService personService,
            IStudioService studioService,
            IImageService  imageService
            )
        {
            _cinemaService = cinemaService;
            _personService = personService;
            _studioService = studioService;
            _imageService  = imageService;
        }

        /// <summary>
        /// Get all cinemas by optional sort and pagination criteria
        /// </summary>
        /// <param name="st">sort parameters</param> 
        /// <param name="pg">pagination parameters</param>
        /// <returns>All cinema list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            string? token = await HttpContext.GetTokenAsync("access_token");
            _cinemaService.Authorize(JwtBearerDefaults.AuthenticationScheme, token);

            Page<CinemasResponse>? response = await _cinemaService.Get(ct, st, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (CinemasResponse cinema in response.Response)
            {
                if(cinema.Picture != null) 
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);  
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all cinemas by indexes 
        /// A method goes under POST variable for avoiding query path constraint
        /// </summary>
        /// <returns>All cinema list</returns>
        /// <param name="ids">indexes to search by</param> 
        /// <param name="st">sort parameters (optional)</param> 
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpPost("indexes")]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync(
            CancellationToken ct,
            [FromBody] string[] ids,
            [FromQuery] SortBy? st = null
            )
        {
            Page<CinemasResponse>? response = await _cinemaService.GetByIds(ids, ct, st);

            if (response is null)
            {
                return NotFound();
            }


            foreach (CinemasResponse cinema in response.Response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Search by name method 
        /// </summary>
        /// <returns>Cinemas list by search string</returns>
        /// <param name="search"> search string </param>
        /// <param name="pg"> pagination parameters (optional) </param>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
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
            IEnumerable<SearchResponse>? response = await _cinemaService.GetBySearch(search, ct, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (SearchResponse cinema in response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Tiny);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Search by name method 
        /// </summary>
        /// <returns>Cinemas list by search string</returns>
        /// <param name="search"> search string </param>
        /// <param name="pg"> pagination parameters (optional) </param>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("search-page/{search}")]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchPage(
            CancellationToken ct,
            [FromRoute] string search,
            [FromQuery] Pagination? pg = null
        )
        {
            Page<CinemasResponse>? response = await _cinemaService.GetBySearchPage(search, ct, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (CinemasResponse cinema in response.Response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all cinemas by provided year with optional sort and pagination criteria
        /// </summary>
        /// <param name="year">year of cinema release</param> 
        /// <param name="st">sort parameters</param> 
        /// <param name="pg">pagination parameters</param>
        /// <returns>All cinema list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("year/{year:int}")]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Year(
            CancellationToken ct,
            [FromRoute] int year,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<CinemasResponse>? response = await _cinemaService.GetByYear(year, ct, st, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (CinemasResponse cinema in response.Response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }


        /// <summary>
        /// Get all studios by provided year with optional sort and pagination criteria
        /// </summary>
        /// <param name="lower">year span lower boundary (>=)</param>
        /// <param name="upper">year span lower boundary (<)</param>
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
            Page<CinemasResponse>? response = await _cinemaService.GetByYearSpans(lys, d, ct, st, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (CinemasResponse cinema in response.Response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all cinemas by provided genres with optional sort and pagination criteria
        /// </summary>
        /// <param name="genres">genres of cinema to search by</param> 
        /// <param name="st">sort parameters</param> 
        /// <param name="pg">pagination parameters</param>
        /// <returns>All cinema list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("genres/{genres}")]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Genres(
            CancellationToken ct,
            [FromRoute] Genre genres,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<CinemasResponse>? response = await _cinemaService.GetByGenre(genres, ct, st, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (CinemasResponse cinema in response.Response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all cinemas by provided language of origin with optional sort and pagination criteria
        /// </summary>
        /// <param name="language">language of cinema origin</param> 
        /// <param name="st">sort parameters</param> 
        /// <param name="pg">pagination parameters</param>
        /// <returns>All cinema list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("language/{language}")]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Language(
            CancellationToken ct,
            [FromRoute] Language language,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<CinemasResponse>? response = await _cinemaService.GetByLanguage(language, ct, st, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (CinemasResponse cinema in response.Response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get all cinemas by provided studio with optional sort and pagination criteria
        /// </summary>
        /// <param name="studioId"> id of studio cinema was filmed by</param>
        /// <param name="st">sort parameters</param> 
        /// <param name="pg">pagination parameters</param>
        /// <returns>All cinema list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("studio/{studioId}")]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Studio(
            CancellationToken ct,
            [FromRoute] string studioId,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            Page<CinemasResponse>? response = await _cinemaService.GetByStudioId(studioId, ct, st, pg);

            if (response is null)
            {
                return NotFound();
            }

            foreach (CinemasResponse cinema in response.Response)
            {
                if (cinema.Picture != null)
                {
                    cinema.PictureUri = await _imageService.GetImage(cinema.Picture, ImageSize.Medium);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Get cinema by its id
        /// </summary>
        /// <param name="id">requested cinema id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CinemaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromRoute] string id
            )
        {
            CinemaResponse? response = await _cinemaService.GetById(id, ct);

            if (response is null)
            {
                return NotFound();
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            if (response.Starrings != null)
            {
                foreach (StarringResponse starring in response.Starrings)
                {
                    if (starring.Picture != null)
                    {
                        starring.PictureUri = await _imageService.GetImage(starring.Picture, ImageSize.Small);
                    }
                }
            }

            if (response.ProductionStudios != null)
            {
                foreach (ProductionStudioResponse studio in response.ProductionStudios)
                {
                    if (studio.Picture != null)
                    {
                        studio.PictureUri = await _imageService.GetImage(studio.Picture, ImageSize.Small);
                    }
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Post new cinema made of request parameter
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Newly created cinema instance</returns>
        /// <response code="200">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(CinemaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(
            CancellationToken ct,
            [FromBody] CreateCinemaRequest request
            )
        {

            Page<StudiosResponse>? studioPage;
            StudiosResponse? studioRecord;

            if (request.ProductionStudios != null && request.ProductionStudios.Any())
            {
                foreach (var studio in request.ProductionStudios)
                {
                    studioPage = await _studioService.GetByIds(new string[] { studio.Id }, ct, null);

                    if (studioPage == null) continue;

                    studioRecord = studioPage.Response.FirstOrDefault();

                    if (studioRecord == null) continue;

                    studio.Name = studioRecord.Name;
                    studio.Picture = studioRecord.Picture;
                }
            }

            Page<PersonsResponse>? personPage;
            PersonsResponse? personRecord;

            if (request.Starrings != null && request.Starrings.Any())
            {
                foreach (var person in request.Starrings)
                {
                    personPage = await _personService.GetByIds(new string[] { person.Id }, ct, null);

                    if (personPage == null) continue;

                    personRecord = personPage.Response.FirstOrDefault();

                    if (personRecord == null) continue;

                    person.Name = personRecord.Name;
                    person.Picture = personRecord.Picture;
                }
            }

            CinemaResponse? response = await _cinemaService.Create(request, ct);

            if (response is null)
                return BadRequest();

            if (request.ProductionStudios != null && request.ProductionStudios.Any())
            {
                foreach (var film in request.ProductionStudios)
                {
                    await _studioService.CreateFilmographyFor(film.Id, new CreateFilmographyRequest
                    {
                        Id = response.Id,
                        Name = response.Name,
                        Picture = response.Picture
                    }, ct);
                }
            }

            if (request.Starrings != null && request.Starrings.Any())
            {
                foreach (var film in request.Starrings)
                {
                    await _personService.CreateFilmographyFor(film.Id, new CreateFilmographyRequest
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
        /// Add to cinema production studios made of request parameter
        /// </summary>
        /// <param name="cinemaId">cinema id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPost("{cinemaId}/production-studios/create")]
        [ProducesResponseType(typeof(ProductionStudioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProductionStudios(
            [FromRoute] string cinemaId,
            [FromBody] CreateProductionStudioRequest request,
            CancellationToken ct
            )
        {
            StudioResponse? studio = await _studioService.GetById(request.Id, ct);

            if (studio is null)
            {
                return NotFound(request.Id);
            }

            request.Name = studio.Name;
            request.Picture = studio.Picture;


            CinemaResponse? cinema = await _cinemaService.GetById(cinemaId, ct);

            if (cinema is null)
            {
                return NotFound(cinemaId);
            }

            ProductionStudioResponse? response = await _cinemaService.CreateProductionStudioFor(cinemaId, request, ct);

            if (response is null)
            {
                return NotFound(request.Id);
            }

            FilmographyResponse? responseBack = await _studioService.CreateFilmographyFor(
                                                    request.Id, 
                                                    new CreateFilmographyRequest { 
                                                        Id = cinema.Id,
                                                        Name = cinema.Name,
                                                        Year = cinema.ReleaseDate.Year,
                                                        Picture = cinema.Picture
                                                    },
                                                    CancellationToken.None);

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Small);
            }

            return Ok(response);
        }

        /// <summary>
        /// Add starring to cinema starring list made of request parameter
        /// </summary>
        /// <param name="cinemaId">cinema id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created starring instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpPost("{cinemaId}/starrings/create")]
        [ProducesResponseType(typeof(StarringResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateStarrings(
            [FromRoute] string cinemaId,
            [FromBody] CreateStarringRequest request,
            CancellationToken ct
            )
        {

            PersonResponse? person = await _personService.GetById(request.Id, ct);

            if (person is null)
            {
                return NotFound(request.Id);
            }

            request.Name = person.Name;
            request.Picture = person.Picture;


            CinemaResponse? cinema = await _cinemaService.GetById(cinemaId, ct);

            if (cinema is null)
            {
                return NotFound(cinemaId);
            }

            StarringResponse? response = null;

            if (cinema.Starrings != null && cinema.Starrings.Any())
            {
                response = cinema.Starrings.FirstOrDefault(s => s.Id == request.Id);

                if(response != null)
                    return Ok(response);
            }

            response = await _cinemaService.CreateStarringFor(cinemaId, request, ct);

            if (response is null)
            {
                return NotFound(request.Id);
            }

            FilmographyResponse? responseBack = await _personService.CreateFilmographyFor(
                                                    request.Id,
                                                    new CreateFilmographyRequest
                                                    {
                                                        Id = cinema.Id,
                                                        Name = cinema.Name,
                                                        Year = cinema.ReleaseDate.Year,
                                                        Picture = cinema.Picture
                                                    },
                                                    CancellationToken.None);

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Small);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update single cinema with provided request parameters
        /// </summary>
        /// <param name="id">id of cinema to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CinemaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            CancellationToken ct,
            [FromRoute] string id,
            [FromBody] UpdateCinemaRequest request
            )
        {
            CinemaResponse? response = await _cinemaService.Update(id, request, ct);

            if (response is null)
            {
                return NotFound(request);
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            await UpdateAdditional(response, ct);

            return Ok(response);
        }

        /// <summary>
        /// Update cinema's main information with provided request parameters
        /// </summary>
        /// <param name="id">id of cinema to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpPut("{id}/main")]
        [ProducesResponseType(typeof(CinemaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMainAsync(
            CancellationToken ct,
            [FromRoute] string id,
            [FromBody] UpdateCinemaRequest request
            )
        {
            CinemaResponse? response = await _cinemaService.UpdateMain(id, request, ct);

            if (response is null)
            {
                return NotFound(request);
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            await UpdateAdditional(response, ct);

            return Ok(response);
        }

        /// <summary>
        /// Update starring for specific cinema
        /// </summary>
        /// <param name="cinemaId">cinema id </param>
        /// <param name="request">request body</param>
        /// <returns>Updated starring instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema or starring to be updated is not found</response>
        [HttpPut("{cinemaId}/starrings/update/{starringId}")]
        [ProducesResponseType(typeof(StarringResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Starrings(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromRoute] string starringId,
            [FromBody] UpdateStarringRequest request
            )
        {
            CinemaResponse? cinema = await _cinemaService.GetById(cinemaId, ct);

            if (cinema is null)
            {
                return NotFound(cinemaId);
            }

            StarringResponse? response = await _cinemaService.UpdateStarring(cinemaId, starringId, request, ct);

            if (response is null)
            {
                return NotFound(starringId);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="cinemaId">id of cinema to be updated</param>
        /// <param name="request">request body with picture</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found or picture was not replaced</response>
        [HttpPut("{cinemaId}/picture/update")]
        [ProducesResponseType(typeof(UpdatePictureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePicture(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromBody] ReplaceImageRequest request
            )
        {
            string? pictureUri;

            var response = await _cinemaService.GetById(cinemaId, ct);

            if (response is null)
            {
                return NotFound(cinemaId);
            }

            if(response.Picture is null)
            {
                pictureUri = await _imageService.AddImage(request.NewId, request.FileBase64, request.Size, ImageSize.Big);
            }
            else pictureUri = await _imageService.ReplaceImage(response.Picture, request.NewId, request.FileBase64, request.Size, ImageSize.Big);

            if (pictureUri == null) 
            {
                return NotFound(request);
            }

            UpdatePictureResponse? responsePhoto = await _cinemaService.UpdatePhoto(cinemaId, new UpdatePictureRequest {
                Picture = request.NewId
            }, ct);

            if (responsePhoto != null && responsePhoto.Picture != null)
            {
                responsePhoto.PictureUri = pictureUri;

                response.Picture = responsePhoto.Picture;
            }

            await UpdateAdditional(response, ct);

            return Ok(responsePhoto);
        }


        /// <summary>
        /// Delete single cinema by its id
        /// </summary>
        /// <param name="id">Id of cinema to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            CancellationToken ct
            )
        {
            CinemaResponse? cinema = await _cinemaService.GetById(id, ct);

            if (cinema is null)
            {
                return NotFound(id);
            }

            var deleted = await _cinemaService.Delete(id, ct);

            if (!deleted) 
            {
                return NotFound(id);
            }

            if (cinema.Starrings != null && cinema.Starrings.Any())
            { 
                await _personService.DeleteFilmography(null, id, ct);            
            }

            if (cinema.ProductionStudios != null && cinema.ProductionStudios.Any())
            {
                await _studioService.DeleteFilmography(null, id, ct);
            }

            if (cinema.Picture != null)
            {
                await _imageService.DeleteImage(cinema.Picture, (ImageSize)31);
            }

            return Ok(id);
        }

        /// <summary>
        /// Delete production studio by optional cinema id
        /// </summary>
        /// <param name="cinemaId">Id of cinema which studio is to be deleted</param>
        /// <param name="studioId">Id of production studio entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Production studio or cinema is not found</response>
        [HttpDelete("{cinemaId}/production-studios/delete/{studioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProductionStudios(
            [FromRoute] string cinemaId,
            [FromRoute] string studioId,
            CancellationToken ct
            )
        {
            CinemaResponse? cinema = await _cinemaService.GetById(cinemaId, ct);

            if (cinema is null) 
            {
                return NotFound(cinemaId);
            }

            bool deleted = await _cinemaService.DeleteProductionStudio(cinemaId, studioId, ct);

            if (!deleted)
            {
                return NotFound(studioId);
            }

            await _studioService.DeleteFilmography(studioId, cinemaId, ct);

            return Ok(studioId);
        }

        /// <summary>
        /// Delete starring by optional cinema id
        /// </summary>
        /// <param name="cinemaId">Id of cinema which starring is to be deleted</param>
        /// <param name="starringId">Id of starrings entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Starring or cinema is not found</response>
        [HttpDelete("{cinemaId}/starrings/delete/{starringId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteStarrings(
            [FromRoute] string cinemaId,
            [FromRoute] string starringId,
            CancellationToken ct
            )
        {
            CinemaResponse? cinema = await _cinemaService.GetById(cinemaId, ct);

            if (cinema is null)
            {
                return NotFound(cinemaId);
            }

            bool deleted = await _cinemaService.DeleteStarring(cinemaId, starringId, ct);

            if (!deleted)
            {
                return NotFound(starringId);
            }

            await _personService.DeleteFilmography(starringId, cinemaId, ct);

            return Ok(starringId);
        }

        protected async Task UpdateAdditional(CinemaResponse cinema, CancellationToken ct)
        {
            UpdateFilmographyRequest? updateRecord = new UpdateFilmographyRequest
            {
                Name = cinema.Name,
                Year = cinema.ReleaseDate.Year,
                Picture = cinema.Picture
            };

            FilmographyResponse? compareRecord = null;

            bool? filmographyCommonsEquals = null;


            if (cinema.Starrings != null && cinema.Starrings.Any())
            {
                compareRecord ??= await _personService.GetFilmographyById(cinema.Starrings.FirstOrDefault().Id, cinema.Id, ct);

                filmographyCommonsEquals ??= updateRecord.SameCommons(compareRecord);

                foreach (StarringResponse starring in cinema.Starrings)
                {
                    if (!filmographyCommonsEquals.GetValueOrDefault())
                    {
                        await _personService.UpdateFilmography(
                            starring.Id,
                            cinema.Id,
                            updateRecord,
                            ct);
                    }

                    if (starring.Picture != null)
                    {
                        starring.PictureUri = await _imageService.GetImage(starring.Picture, ImageSize.Small);
                    }
                }
            }

            if (cinema.ProductionStudios != null && cinema.ProductionStudios.Any())
            {
                compareRecord ??= await _studioService.GetFilmographyById(cinema.ProductionStudios.FirstOrDefault().Id, cinema.Id, ct);

                filmographyCommonsEquals ??= updateRecord.SameCommons(compareRecord);

                foreach (ProductionStudioResponse studio in cinema.ProductionStudios)
                {

                    if (!filmographyCommonsEquals.GetValueOrDefault())
                    {
                        await _studioService.UpdateFilmography(
                        studio.Id,
                        cinema.Id,
                        updateRecord,
                        ct);
                    }

                    if (studio.Picture != null)
                    {
                        studio.PictureUri = await _imageService.GetImage(studio.Picture, ImageSize.Small);
                    }
                }
            }
        }
    }
}
