using Microsoft.AspNetCore.Mvc;
using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Api.Queries.CinemaQueries;
using CinemaDataService.Api.Queries.RecordQueries;
using CinemaDataService.Api.Commands.CinemaCommands.CreateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/cinemas")]
    public class CinemaController : Controller
    {

        private readonly IMediator _mediator;

        private readonly Func<CinemasQuery, CinemasQueryCommonWrapper> _wrapCinemasQuery = (CinemasQuery query) => new CinemasQueryCommonWrapper(query);
        private readonly Func<StarringQuery, StarringQueryCommonWrapper> _wrapStarringQuery = (StarringQuery query) => new StarringQueryCommonWrapper(query);
        private readonly Func<ProductionStudioQuery, ProductionStudioQueryCommonWrapper> _wrapProductionStudioQuery = (ProductionStudioQuery query) => new ProductionStudioQueryCommonWrapper(query);

        public CinemaController(IMediator mediator)
        {
            _mediator = mediator;
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
            var response = await _mediator.Send(_wrapCinemasQuery (new CinemasQuery(st, pg) ), ct);

            return Ok(response);
        }

        /// <summary>
        /// Get all cinemas by indices 
        /// A method goes under POST variable for avoiding query path constraint
        /// </summary>
        /// <returns>All cinema list</returns>
        /// <param name="ids">indices to search by</param> 
        /// <param name="pg">pagination parameters</param> 
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
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(_wrapCinemasQuery (new CinemasIdQuery(ids, pg)), ct);

            return Ok(response);
        }

        /// <summary>
        /// Search by name method 
        /// </summary>
        /// <returns>All cinema list</returns>
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
            var response = await _mediator.Send(new CinemasSearchQuery(search, pg), ct);

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
            var response = await _mediator.Send(_wrapCinemasQuery (new CinemasYearQuery(year, st, pg)), ct);

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
            var response = await _mediator.Send(_wrapCinemasQuery (new CinemasGenresQuery(genres, st, pg)), ct);

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
            var response = await _mediator.Send(_wrapCinemasQuery (new CinemasLanguageQuery(language, st, pg)), ct);

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
            var response = await _mediator.Send(_wrapCinemasQuery (new CinemasStudioQuery(studioId, st, pg)), ct);

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
            var response = await _mediator.Send(new CinemaQuery(id), ct);

            return Ok(response);
        }

        /// <summary>
        /// Get starring by its id and parent cinema id
        /// </summary>
        /// <param name="cinemaId">requested cinema id</param>
        /// <param name="starringId">requested starring id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema or starring is not found</response>
        [HttpGet("{cinemaId}/starrings/{starringId}")]
        [ProducesResponseType(typeof(StarringResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Starring(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromRoute] string starringId
            )
        {
            var response = await _mediator.Send(_wrapStarringQuery(new CinemaStarringQuery(cinemaId, starringId)), ct);

            return Ok(response);
        }

        /// <summary>
        /// Get production studio by its id and parent cinema id
        /// </summary>
        /// <param name="cinemaId">requested cinema id</param>
        /// <param name="studioId">requested studio id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema or production studio is not found</response>
        [HttpGet("{cinemaId}/production-studios/{studioId}")]
        [ProducesResponseType(typeof(ProductionStudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProductionStudio(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromRoute] string studioId
            )
        {
            var response = await _mediator.Send(_wrapProductionStudioQuery(new CinemaProductionStudioQuery(cinemaId, studioId)), ct);

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
            var response = await _mediator.Send(
                new CreateCinemaCommand(
                        request.Name,
                        request.ReleaseDate,
                        request.Genres,
                        request.Language,
                        request.Picture,
                        request.ProductionStudios,
                        request.Starrings,
                        request.Description
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Add to cinema production studios made of request parameter
        /// </summary>
        /// <param name="cinemaId">cinema id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{cinemaId}/production-studios")]
        [ProducesResponseType(typeof(ProductionStudioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProductionStudios(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromBody] CreateProductionStudioRequest request
            )
        {
            var response = await _mediator.Send(
                new CreateCinemaProductionStudioCommand(
                        cinemaId,
                        request.Id,
                        request.Name,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Add starring to cinema starring list made of request parameter
        /// </summary>
        /// <param name="cinemaId">cinema id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created starring instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{cinemaId}/starrings")]
        [ProducesResponseType(typeof(StarringResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Starrings(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromBody] CreateStarringRequest request
            )
        {
            var response = await _mediator.Send(
                new CreateCinemaStarringCommand(
                        cinemaId,
                        request.Id,
                        request.Name,
                        request.Jobs,
                        request.RoleName,
                        request.RolePriority,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update cinema main information with some of provided request parameters
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
            var response = await _mediator.Send(
                new UpdateCinemaMainCommand(
                        id,
                        request.Name,
                        request.ReleaseDate,
                        request.Genres,
                        request.Language,
                        request.Description
                    ),
                ct
                );

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
            var response = await _mediator.Send(
                new UpdateCinemaCommand(
                        id,
                        request.Name,
                        request.ReleaseDate,
                        request.Genres,
                        request.Language,
                        request.Picture,
                        request.ProductionStudios,
                        request.Starrings,
                        request.Description
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="cinemaId">id of cinema to be updated</param>
        /// <param name="request">request body with picture</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpPut("{cinemaId}/picture")]
        [ProducesResponseType(typeof(UpdatePictureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Picture(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromBody] UpdatePictureRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateCinemaPictureCommand(
                        cinemaId,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update cinema rating
        /// </summary>
        /// <param name="cinemaId">id of cinema to be updated</param>
        /// <param name="request">request body with rating</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpPut("{cinemaId}/rating")]
        [ProducesResponseType(typeof(UpdateRatingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Rating(
            CancellationToken ct,
            [FromRoute] string cinemaId,
            [FromBody] UpdateRatingRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateCinemaRatingCommand(
                        cinemaId,
                        request.Rating,
                        request.OldRating
                    ),
                ct
                );

            return Ok(response);
        }


        /// <summary>
        /// Update production studio for all cinemas or specific cinema
        /// </summary>
        /// <param name="cinemaId">cinema id (optional)</param>
        /// <param name="studioId">production studio id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPut("production-studios/{studioId}")]
        [HttpPut("{cinemaId}/production-studios/{studioId}")]
        [ProducesResponseType(typeof(ProductionStudioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProductionStudios(
            CancellationToken ct,
            [FromRoute] string? cinemaId,
            [FromRoute] string studioId,
            [FromBody] UpdateProductionStudioRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateCinemaProductionStudioCommand(
                        cinemaId,
                        studioId,
                        request.Name,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update starring for all cinemas or specific cinema
        /// </summary>
        /// <param name="cinemaId">cinema id (optional)</param>
        /// <param name="request">request body</param>
        /// <returns>Updated starring instance</returns>
        /// <response code="200">Success</response>
        [HttpPut("starrings/{starringId}")]
        [HttpPut("{cinemaId}/starrings/{starringId}")]
        [ProducesResponseType(typeof(StarringResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Starrings(
            CancellationToken ct,
            [FromRoute] string? cinemaId,
            [FromRoute] string starringId,
            [FromBody] UpdateStarringRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateCinemaStarringCommand(
                        cinemaId,
                        starringId,
                        request.Name,
                        request.Jobs,
                        request.RoleName,
                        request.RolePriority,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
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
            CancellationToken ct,
            [FromRoute] string id
            )
        {
            await _mediator.Send(new DeleteCinemaCommand(id), ct);

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
        [HttpDelete("production-studios/{studioId}")]
        [HttpDelete("{cinemaId}/production-studios/{studioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProductionStudios(
            CancellationToken ct,
            [FromRoute] string? cinemaId,
            [FromRoute] string studioId
            )
        {
            await _mediator.Send(new DeleteCinemaProductionStudioCommand(cinemaId, studioId), ct);

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
        [HttpDelete("starrings/{starringId}")]
        [HttpDelete("{cinemaId}/starrings/{starringId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Starrings(
            CancellationToken ct,
            [FromRoute] string? cinemaId,
            [FromRoute] string starringId
            )
        {
            await _mediator.Send(new DeleteCinemaStarringCommand(cinemaId, starringId), ct);

            return Ok(starringId);
        }
    }
}
