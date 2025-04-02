using MediatR;
using Microsoft.AspNetCore.Mvc;
using CinemaDataService.Api.Commands.StudioCommands;
using CinemaDataService.Api.Queries.StudioQueries;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IMediator _mediator;

        public StudioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all studios by optional criteria
        /// </summary>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No task was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListAsync(
            [FromQuery] string? name = null,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new StudiosQuery(st, pg));

            return Ok(response);
        }

        /// <summary>
        /// Get user task by its id
        /// </summary>
        /// <param name="id">requested studio id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            [FromQuery] string id)
        {
            var response = await _mediator.Send(
                new StudioQuery(id)
                );

            return Ok(response);
        }

        /// <summary>
        /// Post new task made of request parameter
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(
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
                    )
                );

            return Ok(response);
        }

        /// <summary>
        /// Update single task with provided request parameters
        /// </summary>
        /// <param name="id">id of studio to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPut]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromQuery] string id,
            [FromBody] UpdateStudioRequest request)
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
                    )
                );

            return Ok(response);
        }

        /// <summary>
        /// Delete single task by its name
        /// </summary>
        /// <param name="id">Id of studio to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id
            )
        {
            await _mediator.Send(new DeleteStudioCommand(id));

            return Ok();
        }
    }
}
