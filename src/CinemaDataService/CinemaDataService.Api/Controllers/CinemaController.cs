using Microsoft.AspNetCore.Mvc;
using MediatR;
using CinemaDataService.Api.Commands.CinemaCommands;
using CinemaDataService.Api.Queries.CinemaQueries;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CinemaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IMediator _mediator;

        public CinemaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all cinemas by optional criteria
        /// </summary>
        /// <param name="st">sort parameters</param> 
        /// <param name="pg">pagination parameters</param>
        /// <returns>All cinema list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No cinema was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CinemaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListAsync(
            [FromQuery] string? name = null,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new CinemasQuery(st, pg));

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
            [FromQuery] string id)
        {
            var response = await _mediator.Send(new CinemaQuery(id));

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
                    )
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
        [HttpPut]
        [ProducesResponseType(typeof(CinemaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] string id,
            [FromBody] UpdateCinemaRequest request)
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
                    )
                );

            return Ok(response);
        }

        /// <summary>
        /// Delete single cinema by its name
        /// </summary>
        /// <param name="id">Id of cinema to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Cinema is not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromQuery] string id)
        {
            await _mediator.Send(new DeleteCinemaCommand(id));

            return Ok();
        }
    }
}
