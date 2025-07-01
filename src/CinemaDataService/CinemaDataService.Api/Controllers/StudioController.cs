using CinemaDataService.Api.Commands.StudioCommands.CreateCommands;
using CinemaDataService.Api.Commands.StudioCommands.DeleteCommands;
using CinemaDataService.Api.Commands.StudioCommands.UpdateCommands;
using CinemaDataService.Api.Queries.CinemaQueries;
using CinemaDataService.Api.Queries.RecordQueries;
using CinemaDataService.Api.Queries.StudioQueries;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/studios")]
    public class StudioController : Controller
    {

        private readonly IMediator _mediator;
        private readonly Func<StudiosQuery, StudiosQueryCommonWrapper> _wrapStudiosQuery = (StudiosQuery query) => new StudiosQueryCommonWrapper(query);
        private readonly Func<FilmographyQuery, FilmographyQueryCommonWrapper> _wrapFilmographyQuery = (FilmographyQuery query) => new FilmographyQueryCommonWrapper(query);
        public StudioController(IMediator mediator)
        {
            _mediator = mediator;
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
            var response = await _mediator.Send(_wrapStudiosQuery( new StudiosQuery(st, pg)), ct);

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
            var response = await _mediator.Send(new StudiosSearchQuery(search, pg), ct);

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
            var response = await _mediator.Send(_wrapStudiosQuery( new StudiosYearQuery(year, st, pg)), ct);

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
            var response = await _mediator.Send(_wrapStudiosQuery( new StudiosCountryQuery(country, st, pg)), ct);

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
            var response = await _mediator.Send(
                new StudioQuery(id), ct
                );

            return Ok(response);
        }


        /// <summary>
        /// Get all studios by indices 
        /// A method goes under POST variable for avoiding query path constraint
        /// </summary>
        /// <returns>All studio list</returns>
        /// <param name="ids">indices to search by</param> 
        /// <param name="pg">pagination parameters</param> 
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpPost("indexes")]
        [ProducesResponseType(typeof(Page<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync(
            CancellationToken ct,
            [FromBody] string[] ids,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(_wrapStudiosQuery(new StudiosIdQuery(ids, pg)), ct);

            return Ok(response);
        }

        /// <summary>
        /// Get filmography by its id and parent studio id
        /// </summary>
        /// <param name="studioId">requested studio id</param>
        /// <param name="filmographyId">requested filmography id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person or filmography is not found</response>
        [HttpGet("{studioId}/filmography/{filmographyId}")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmography(
            CancellationToken ct,
            [FromRoute] string studioId,
            [FromRoute] string filmographyId
            )
        {
            var response = await _mediator.Send(_wrapFilmographyQuery(new StudioFilmographyQuery(studioId, filmographyId)), ct);

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
            var response = await _mediator.Send(
                new CreateStudioCommand(
                        request.Name,
                        request.FoundDate,
                        request.Country,
                        request.Picture,
                        request.Filmography,
                        request.PresidentName,
                        request.Description
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Add cinema to studio filmography made of request parameter
        /// </summary>
        /// <param name="studioId">studio id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{studioId}/filmography")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Filmography(
            CancellationToken ct,
            [FromRoute] string studioId,
            [FromBody] CreateFilmographyRequest request
            )
        {
            var response = await _mediator.Send(
                new CreateStudioFilmographyCommand(
                        studioId,
                        request.Id,
                        request.Name,
                        request.Year,
                        request.Picture
                    ),
                ct
                );

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
            CancellationToken ct,
            [FromRoute] string id,
            [FromBody] UpdateStudioRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateStudioCommand(
                        id,
                        request.Name,
                        request.FoundDate,
                        request.Country,
                        request.Picture,
                        request.Filmography,
                        request.PresidentName,
                        request.Description
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update main studio information with some of provided request parameters
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
            CancellationToken ct,
            [FromRoute] string id,
            [FromBody] UpdateStudioRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateStudioMainCommand(
                        id,
                        request.Name,
                        request.FoundDate,
                        request.Country,
                        request.PresidentName,
                        request.Description
                    ),
                ct
                );

            return Ok(response);
        }


        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="id">id of studio to be updated</param>
        /// <param name="request">request body with picture</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpPut("{id}/picture")]
        [ProducesResponseType(typeof(UpdatePictureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            CancellationToken ct,
            [FromRoute] string id,
            [FromBody] UpdatePictureRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateStudioPictureCommand(
                        id,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update filmography entrance for all studios
        /// </summary>
        /// <param name="studioId">id of studio which filmography entrance to be updated (optional)</param>
        /// <param name="filmographyId">id of filmography entrance to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPut("filmography/{filmographyId}")]
        [HttpPut("{studioId}/filmography/{filmographyId}")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmography(
            CancellationToken ct,
            [FromRoute] string? studioId,
            [FromRoute] string filmographyId,
            [FromBody] UpdateFilmographyRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateStudioFilmographyCommand(
                        studioId,
                        filmographyId,
                        request.Name,
                        request.Year,
                        request.Picture
                    ),
                ct
                );

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
            await _mediator.Send(new DeleteStudioCommand(id), ct);

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
        [HttpDelete("filmography/{id}")]
        [HttpDelete("{studioId}/filmography/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmographys(
            CancellationToken ct,
            [FromRoute] string? studioId,
            [FromRoute] string id
            )
        {
            await _mediator.Send(new DeleteStudioFilmographyCommand(studioId, id), ct);

            return Ok();
        }
    }
}
