using MediatR;
using Microsoft.AspNetCore.Mvc;
using CinemaDataService.Api.Queries.PersonQueries;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Api.Commands.PersonCommands.CreateCommands;
using CinemaDataService.Api.Commands.PersonCommands.UpdateCommands;
using CinemaDataService.Api.Commands.PersonCommands.DeleteCommands;
using CinemaDataService.Infrastructure.Repositories.Utils;
using CinemaDataService.Api.Queries.CinemaQueries;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonController : Controller
    {

        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all persons by optional sort and pagination criteria
        /// </summary>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All persons list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No person was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new PersonsQuery(st, pg), ct);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            [FromQuery] string search,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null,
            CancellationToken ct = default
        )
        {
            var response = await _mediator.Send(new PersonsSearchQuery(search, st, pg), ct);

            return Ok(response);
        }

        /// <summary>
        /// Get all persons by country with optional sort and pagination criteria
        /// </summary>
        /// <param name="country">country of origin</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All persons list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No person was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("country/{country}")]
        [ProducesResponseType(typeof(IEnumerable<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Country(
            [FromRoute] Country country,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new PersonsCountryQuery(country, st, pg), ct);

            return Ok(response);
        }

        /// <summary>
        /// Get all persons by jobs with optional sort and pagination criteria
        /// </summary>
        /// <param name="jobs">jobs of persons to search for</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All persons list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No person was found</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("jobs/{jobs}")]
        [ProducesResponseType(typeof(IEnumerable<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Jobs(
            [FromRoute] Job jobs,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null,
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new PersonsJobsQuery(jobs, st, pg), ct);

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
            [FromRoute] string id, 
            CancellationToken ct = default)
        {
            var response = await _mediator.Send(new PersonQuery(id), ct);

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
            [FromBody] CreatePersonRequest request, 
            CancellationToken ct = default
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
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Add cinema to person filmography made of request parameter
        /// </summary>
        /// <param name="personId">person id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created filmography instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{personId}/filmography")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Filmography(
            [FromRoute] string personId,
            [FromBody] CreateFilmographyRequest request, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(
                new CreatePersonFilmographyCommand(
                        personId,
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
        /// Update single person with provided request parameters
        /// </summary>
        /// <param name="id">id of person to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated person instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromQuery] string id,
            [FromBody] UpdatePersonRequest request, 
            CancellationToken ct = default)
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
                new UpdatePersonPictureCommand(
                        id,
                        request.Picture
                    ),
                ct
                );

            return Ok(response);
        }

        /// <summary>
        /// Update filmography entrance for all persons
        /// </summary>
        /// <param name="id">id of filmography entrance to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPut("filmography/{id}")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmography(
            [FromRoute] string id,
            [FromBody] UpdateFilmographyRequest request, 
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(
                new UpdatePersonFilmographyCommand(
                        id,
                        request.Name,
                        request.Year,
                        request.Picture
                    ),
                ct
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
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id, 
            CancellationToken ct = default)
        {
            await _mediator.Send(new DeletePersonCommand(id), ct);

            return Ok();
        }

        /// <summary>
        /// Delete single filmography entrance by optional person id
        /// </summary>
        /// <param name="personId">Id of person which filmography cinema to be deleted</param>
        /// <param name="id">Id of filmography entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Person or cinema is not found</response>
        [HttpDelete("filmography/{id}")]
        [HttpDelete("{studioId}/filmography/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmography(
            [FromRoute] string? personId,
            [FromRoute] string id,
            CancellationToken ct = default
            )
        {
            await _mediator.Send(new DeletePersonFilmographyCommand(personId, id), ct);

            return Ok();
        }
    }
}
