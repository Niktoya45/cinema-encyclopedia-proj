using MediatR;
using Microsoft.AspNetCore.Mvc;
using CinemaDataService.Api.Commands.PersonCommands;
using CinemaDataService.Api.Queries.PersonQueries;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all persons
        /// </summary>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All persons list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No person was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListAsync(
            [FromQuery] string? name = null,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new PersonsQuery(st, pg));

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
            [FromQuery] string id)
        {
            var response = await _mediator.Send(new PersonQuery(id));

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
            [FromBody] CreatePersonRequest request
            )
        {
            var response = await _mediator.Send(
                new CreatePersonCommand(
                        request.Name,
                        request.BirthDate,
                        request.Country,
                        request.Jobs,
                        request.Picture,
                        request.Filmography,
                        request.Description
                    )
                );

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
        [HttpPut]
        [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromQuery] string id,
            [FromBody] UpdatePersonRequest request)
        {
            var response = await _mediator.Send(
                new UpdatePersonCommand(
                        id,
                        request.Name,
                        request.BirthDate,
                        request.Country,
                        request.Jobs,
                        request.Picture,
                        request.Filmography,
                        request.Description
                    )
                );

            return Ok(response);
        }

        /// <summary>
        /// Delete single person by its id
        /// </summary>
        /// <param name="id">Id of person to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromQuery] string id)
        {
            await _mediator.Send(new DeletePersonCommand(id));

            return Ok();
        }
    }
}
