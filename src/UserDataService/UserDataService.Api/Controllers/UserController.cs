using Microsoft.AspNetCore.Mvc;
using MediatR;
using UserDataService.Api.Commands.UserCommands;
using UserDataService.Api.Queries.UserQueries;
using UserDataService.Infrastructure.Models.UserDTO;
using UserDataService.Domain.Aggregates.UserAggregate;

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
        /// <param name="userId">authorized user Id</param>
        /// <param name="id">requested User id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string userId,
            [FromQuery] string id)
        {
            var response = await _mediator.Send(new UserQuery(id, userId));

            return Ok(response);
        }

        /// <summary>
        /// Post new user made of request parameter
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created user instance</returns>
        /// <response code="200">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateUserRequest request
            )
        {
            var response = await _mediator.Send(new CreateUserCommand(
                                            request.Username, 
                                            request.Birthdate,
                                            request.Picture));

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
            [FromBody] UpdateUserRequest request)
        {
            var response = await _mediator.Send(new UpdateUserCommand(
                                        userId,
                                        request.Username,
                                        request.Birthdate,
                                        request.Picture
                ));

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
            [FromRoute] string userId
            )
        {
            await _mediator.Send(new DeleteUserCommand(userId));

            return Ok();
        }
    }
}
