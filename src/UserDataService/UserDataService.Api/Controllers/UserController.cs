using Microsoft.AspNetCore.Mvc;
using MediatR;
using UserDataService.Api.Commands.UserCommands.CreateCommands;
using UserDataService.Api.Commands.UserCommands.UpdateCommands;
using UserDataService.Api.Commands.UserCommands.DeleteCommands;
using UserDataService.Api.Queries.UserQueries;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Infrastructure.Models.LabeledDTO;
using UserDataService.Infrastructure.Models.RatingDTO;

namespace UserDataService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get user by its id
        /// </summary>
        /// <param name="id">requested User id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string id,
            CancellationToken ct = default)
        {
            var response = await _mediator.Send(new UserQuery(id), ct);

            return Ok(response);
        }

        /// <summary>
        /// Get user by its id
        /// </summary>
        /// <param name="id">authorized user Id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpGet("{id}/label")]
        [ProducesResponseType(typeof(IEnumerable<LabeledResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Labeled(
            [FromRoute] string id,
            CancellationToken ct = default)
        {
            var response = await _mediator.Send(new UserLabeledQuery(id), ct);

            return Ok(response);
        }

        /// <summary>
        /// Post new user made of request parameter
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Newly created user instance</returns>
        /// <response code="200">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateUserRequest request,
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CreateUserCommand(
                                            request.Username,
                                            request.Birthdate,
                                            request.Picture,
                                            request.Description), ct);

            return Ok(response);
        }

        /// <summary>
        /// Post new user made of request parameter
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created user instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{userId}/label")]
        [ProducesResponseType(typeof(LabeledResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Labeled(
            [FromRoute] string userId,
            [FromBody] CreateLabeledRequest request,
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CreateLabeledCommand(
                                        userId, 
                                        request.CinemaId, 
                                        request.Label), ct);

            return Ok(response);
        }

        /// <summary>
        /// Post new user made of request parameter
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created user instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{userId}/rating")]
        [ProducesResponseType(typeof(RatingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Rating(
            [FromRoute] string userId,
            [FromBody] CreateRatingRequest request,
            CancellationToken ct = default
            )
        {
            var response = await _mediator.Send(new CreateRatingCommand(
                                        userId,
                                        request.CinemaId,
                                        request.Rating), ct);

            return Ok(response);
        }

        /// <summary>
        /// Update single user with provided request parameters
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="request">request body</param>
        /// <returns>Updated user instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpPut]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] string userId,
            [FromBody] UpdateUserRequest request,
            CancellationToken ct = default)
        {
            var response = await _mediator.Send(new UpdateUserCommand(
                                        userId,
                                        request.Username,
                                        request.Birthdate,
                                        request.Picture,
                                        request.Description), ct);

            return Ok(response);
        }

        /// <summary>
        /// Delete single user by its name
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string userId,
            CancellationToken ct = default
            )
        {
            await _mediator.Send(new DeleteUserCommand(userId), ct);

            return Ok();
        }

        /// <summary>
        /// Delete single user by its name
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="cinemaId">existing cinema Id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpDelete("{userId}/label/{cinemaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Labeled(
            [FromRoute] string userId,
            [FromRoute] string cinemaId,
            CancellationToken ct = default
            )
        {
            await _mediator.Send(new DeleteLabeledCommand(userId, cinemaId), ct);

            return Ok();
        }

        /// <summary>
        /// Delete single user by its name
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="cinemaId">existing cinema Id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpDelete("{userId}/rating/{cinemaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Rating(
            [FromRoute] string userId,
            [FromRoute] string cinemaId,
            CancellationToken ct = default
            )
        {
            await _mediator.Send(new DeleteLabeledCommand(userId, cinemaId), ct);

            return Ok();
        }

    }
}
