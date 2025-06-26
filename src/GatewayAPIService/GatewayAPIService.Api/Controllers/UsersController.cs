using GatewayAPIService.Infrastructure.Services.CinemaService;
using GatewayAPIService.Infrastructure.Services.UserService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.UserDataService.Models.Flags;
using Shared.UserDataService.Models.LabeledDTO;
using Shared.UserDataService.Models.RatingDTO;
using Shared.UserDataService.Models.UserDTO;

namespace GatewayAPIService.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        public readonly ICinemaService _cinemaService;
        public readonly IUserService _userService;

        public UserController(
            ICinemaService cinemaService,
            IUserService userService
            )
        {
            _cinemaService = cinemaService;
            _userService = userService;
        }

        /// <summary>
        /// Get user by its id
        /// </summary>
        /// <param name="id">requested User id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string userId,
            CancellationToken ct = default)
        {
            var response = await _userService.Get(userId, ct);

            if (response is null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        /// <summary>
        /// Get labeled records by user id and optionally label 
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="label">label to search list for</param>
        /// <param name="cinemaId">id of searched cinema (optional)</param>
        /// <param name="ct">cancellation token</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpGet("{userId}/label")]
        [HttpGet("{userId}/label/{label}")]
        [ProducesResponseType(typeof(Page<CinemasResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Labeled(
            [FromRoute] string userId,
            [FromRoute] Label? label = null,
            [FromQuery] string? cinemaId = null,
            CancellationToken ct = default)
        {
            var responseLabeled = cinemaId is null ?
                await _userService.GetLabelled(userId, label, ct)
              : await _userService.GetLabelFor(userId, cinemaId, ct);

            if (responseLabeled is null)
            {
                return BadRequest();
            }

            var responseCinemas = await _cinemaService.GetByIds(
                responseLabeled.Select(l => l.CinemaId).ToArray(),
                ct, 
                null);

            if (responseCinemas is null)
            {
                return BadRequest();
            }

            LabeledCinemasResponse<CinemasResponse> response = new LabeledCinemasResponse<CinemasResponse>
            {
                LabeledCinemas = Enumerable.Range(0, responseLabeled.Count())
                                    .Select(i => 
                                        new LabeledCinemaResponse<CinemasResponse> 
                                        { 
                                            Cinema = responseCinemas.Response.ElementAt(i),
                                            Label = responseLabeled.ElementAt(i).Label,
                                        }).ToList(),
                UserId = userId
            };

            return Ok(responseCinemas);
        }

        /// <summary>
        /// Get rating for certain cinema 
        /// </summary>
        /// <param name="id">authorized user Id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpGet("{userId}/rating/{cinemaId}")]
        [ProducesResponseType(typeof(RatingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Rating(
            [FromRoute] string userId,
            [FromRoute] string cinemaId,
            CancellationToken ct = default)
        {
            var response = await _userService.GetRatingFor(userId, cinemaId, ct);

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
            var response = await _userService.Create(request, ct);

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
            var response = await _userService.CreateForLabeledList(userId, request, ct);

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
            var response = await _userService.CreateForRatingList(userId, request, ct);

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
        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] string userId,
            [FromBody] UpdateUserRequest request,
            CancellationToken ct = default)
        {
            var response = await _userService.Update(userId, request, ct);

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
            var response = await _userService.Delete(userId, ct);

            if (!response)
            {
                return BadRequest();
            }

            await _userService.DeleteFromLabeledList(userId, null, ct);

            return Ok(userId);
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
            var response = await _userService.DeleteFromLabeledList(userId, cinemaId, ct);

            if (!response)
            {
                return BadRequest();
            }    

            return Ok(cinemaId);
        }

    }
}
