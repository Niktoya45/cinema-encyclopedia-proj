using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.PersonDTO;
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.SharedDTO;

namespace GatewayAPIService.Api.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : Controller
    {

        private readonly IMediator _mediator;

        public PersonsController(IMediator mediator)
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
        [ProducesResponseType(typeof(Page<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            CancellationToken ct,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new PersonsQuery(st, pg), ct);

            return Ok(response);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<SearchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search(
            CancellationToken ct,
            [FromQuery] string search,
            [FromQuery] Pagination? pg = null
        )
        {
            var response = await _mediator.Send(new PersonsSearchQuery(search, pg), ct);

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
        [ProducesResponseType(typeof(Page<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Country(
            CancellationToken ct,
            [FromRoute] Country country,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
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
        [ProducesResponseType(typeof(Page<PersonsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Jobs(
            CancellationToken ct,
            [FromRoute] Job jobs,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
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
            CancellationToken ct,
            [FromRoute] string id
            )
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
            CancellationToken ct,
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
            CancellationToken ct,
            [FromRoute] string personId,
            [FromBody] CreateFilmographyRequest request
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
            CancellationToken ct,
            [FromQuery] string id,
            [FromBody] UpdatePersonRequest request
            )
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

        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="id">Id of person to be updated</param>
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
            CancellationToken ct,
            [FromRoute] string id,
            [FromBody] UpdateFilmographyRequest request
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
            CancellationToken ct,
            [FromRoute] string id
            )
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
            CancellationToken ct,
            [FromRoute] string? personId,
            [FromRoute] string id
            )
        {
            await _mediator.Send(new DeletePersonFilmographyCommand(personId, id), ct);

            return Ok();
        }
    }
}
