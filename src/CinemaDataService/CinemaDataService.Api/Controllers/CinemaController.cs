using Microsoft.AspNetCore.Mvc;
using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Api.Queries.CinemaQueries;
using CinemaDataService.Api.Commands.CinemaCommands.CreateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Utils;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/cinemas")]
    public class CinemaController : Controller
    {

        private readonly IMediator _mediator;

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
        [ProducesResponseType(typeof(IEnumerable<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CinemasQuery(st, pg), ct);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            [FromQuery] string search,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null,
            CancellationToken ct = default
        )
        {
            var response = await _mediator.Send(new CinemasSearchQuery(search, st, pg), ct);

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
        [ProducesResponseType(typeof(IEnumerable<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Year(
            [FromRoute] int year,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CinemasYearQuery(year, st, pg), ct);

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
        [ProducesResponseType(typeof(IEnumerable<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Genres(
            [FromRoute] Genre genres,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CinemasGenresQuery(genres, st, pg), ct);

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
        [ProducesResponseType(typeof(IEnumerable<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Language(
            [FromRoute] Language language,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CinemasLanguageQuery(language, st, pg), ct);

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
        [ProducesResponseType(typeof(IEnumerable<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Studio(
            [FromRoute] string studioId,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CinemasStudioQuery(studioId, st, pg), ct);

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
            [FromRoute] string id,
            CancellationToken ct = default)
        {
            var response = await _mediator.Send(new CinemaQuery(id), ct);

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
            [FromBody] CreateCinemaRequest request, 
            CancellationToken ct = default
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
        [HttpPost("{cinemaId}/production_studios")]
        [ProducesResponseType(typeof(ProductionStudioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProductionStudios(
            [FromRoute] string cinemaId,
            [FromBody] CreateProductionStudioRequest request, 
            CancellationToken ct = default
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
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{cinemaId}/starrings")]
        [ProducesResponseType(typeof(StarringResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Starrings(
            [FromRoute] string cinemaId,
            [FromBody] CreateStarringRequest request, 
            CancellationToken ct = default
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
            [FromRoute] string id,
            [FromBody] UpdateCinemaRequest request, 
            CancellationToken ct = default)
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

        [HttpPut("{id}/picture")]
        [ProducesResponseType(typeof(UpdatePictureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] string id,
            [FromBody] UpdatePictureRequest request,
            CancellationToken ct = default)
        {
            var response = await _mediator.Send(
                new UpdateCinemaPictureCommand(
                        id,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update production studio for all cinemas
        /// </summary>
        /// <param name="id">production studio id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPut("production_studios/{id}")]
        [ProducesResponseType(typeof(ProductionStudioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProductionStudios(
            [FromRoute] string id,
            [FromBody] UpdateProductionStudioRequest request, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(
                new UpdateCinemaProductionStudioCommand(
                        id,
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
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPut("starrings/{id}")]
        [HttpPut("{cinemaId}/starrings/{id}")]
        [ProducesResponseType(typeof(StarringResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Starrings(
            [FromRoute] string? cinemaId,
            [FromRoute] string id,
            [FromBody] UpdateStarringRequest request, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(
                new UpdateCinemaStarringCommand(
                        cinemaId,
                        id,
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
            [FromRoute] string id,
            CancellationToken ct = default)
        {
            await _mediator.Send(new DeleteCinemaCommand(id), ct);

            return Ok();
        }

        /// <summary>
        /// Delete production studio by optional cinema id
        /// </summary>
        /// <param name="cinemaId">Id of cinema which studio is to be deleted</param>
        /// <param name="id">Id of production studio entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Production studio or cinema is not found</response>
        [HttpDelete("production_studios/{id}")]
        [HttpDelete("{cinemaId}/production_studios/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProductionStudios(
            [FromRoute] string? cinemaId,
            [FromRoute] string id,
            CancellationToken ct = default
            )
        {
            await _mediator.Send(new DeleteCinemaProductionStudioCommand(cinemaId, id), ct);

            return Ok();
        }

        /// <summary>
        /// Delete starring by optional cinema id
        /// </summary>
        /// <param name="cinemaId">Id of cinema which starring is to be deleted</param>
        /// <param name="id">Id of starrings entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Starring or cinema is not found</response>
        [HttpDelete("starrings/{id}")]
        [HttpDelete("{cinemaId}/starrings/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Starrings(
            [FromRoute] string? cinemaId,
            [FromRoute] string id,
            CancellationToken ct = default
            )
        {
            await _mediator.Send(new DeleteCinemaStarringCommand(cinemaId, id), ct);

            return Ok();
        }
    }
}
