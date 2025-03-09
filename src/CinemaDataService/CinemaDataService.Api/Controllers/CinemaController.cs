using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
        /// Get all tasks of specific user
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All user task list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No task was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetCinemaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListAsync(
            [FromRoute] string userId,
            [FromQuery] string? name = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new CinemaQuery( pg));

            return Ok(response);
        }

        /// <summary>
        /// Get user task by its id
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="id">requested task id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Task is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetCinemaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTaskAsync(
            [FromRoute] string userId,
            [FromQuery] string id)
        {
            var response = await _mediator.Send(new CinemaQuery( id));

            return Ok(response);
        }

        /// <summary>
        /// Post new task made of request parameter
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created task instance</returns>
        /// <response code="200">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateCinemaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(
            [FromRoute] string userId,
            [FromBody] CreateCinemaRequest request
            )
        {
            var response = await _mediator.Send(new CreateCinemaCommand());

            return Ok(response);
        }

        /// <summary>
        /// Update single task with provided request parameters
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Task is not found</response>
        [HttpPut]
        [ProducesResponseType(typeof(UpdateCinemaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] string userId,
            [FromBody] UpdateCinemaRequest request)
        {
            var response = await _mediator.Send(new UpdateCinemaCommand());

            return Ok(response);
        }

        /// <summary>
        /// Delete single task by its name
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Task is not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string userId,
            [FromQuery] string name)
        {
            await _mediator.Send(new DeleteCinemaCommand(userId));

            return Ok();
        }
    }
}
