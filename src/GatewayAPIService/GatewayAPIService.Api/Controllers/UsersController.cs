using GatewayAPIService.Infrastructure.Services.AccessService;
using GatewayAPIService.Infrastructure.Services.CinemaService;
using GatewayAPIService.Infrastructure.Services.ImageService;
using GatewayAPIService.Infrastructure.Services.UserService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.CinemaDataService.Models.CinemaDTO;
using Shared.CinemaDataService.Models.SharedDTO;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;
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
        public readonly IImageService _imageService;
        public readonly IUserService _userService;
        public readonly IAccessService _accessService;

        public UserController(
            ICinemaService cinemaService,
            IImageService imageService,
            IUserService userService,
            IAccessService accessService
            )
        {
            _cinemaService = cinemaService;
            _imageService = imageService;
            _userService = userService;
            _accessService = accessService;
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
                return NotFound();
            }

            if (response.Picture != null)
            {
                response.PictureUri = await _imageService.GetImage(response.Picture, ImageSize.Big);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get user role by user id
        /// </summary>
        /// <param name="userId">requested User id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpGet("{userId}/role")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoleAsync(
            [FromRoute] string userId,
            CancellationToken ct = default)
        {
            var response = await _accessService.GetRole(userId, ct);

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
                                            AddedAt = responseLabeled.ElementAt(i).AddedAt
                                        }).ToList(),
                UserId = userId
            };

            return Ok(response);
        }

        /// <summary>
        /// Get rating for certain cinema 
        /// </summary>
        /// <param name="userId">authorized user Id</param>
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
            var responseLabeled = await _userService.GetLabelFor(userId, request.CinemaId, ct);

            if (responseLabeled != null && responseLabeled.Any())
            {
                Label currentLabel = responseLabeled.FirstOrDefault().Label;

                // cinema is not labelled with provided label already
                if ((currentLabel & request.Label) == 0)
                {
                    if ((request.Label & Label.Favored) != 0)
                    {
                        request.Label |= currentLabel;
                    }
                    else if ((currentLabel & Label.Favored) != 0)
                    {
                        request.Label |= Label.Favored;
                    }
                }
            }

            var responseCinema = await _cinemaService.GetByIds(new string[] { request.CinemaId }, ct, null);

            if (responseCinema is null || !responseCinema.Response.Any())
            {
                return NotFound();
            }

            var response = await _userService.CreateForLabeledList(userId, request, ct);

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
        /// Post new user made of request parameter
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created user instance</returns>
        /// <response code="200">Success</response>
        [HttpPut("{userId}/rating")]
        [ProducesResponseType(typeof(RatingResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Rating(
            [FromRoute] string userId,
            [FromBody] UpdateRatingRequest request,
            CancellationToken ct = default
            )
        {
            var responseOldRating = await _userService.GetRatingFor(userId, request.Id, ct);

            request.OldRating = responseOldRating is null ? 0 : responseOldRating.Rating;

            var responseCinemaRating = await _cinemaService.UpdateRating(request.Id, request, ct);

            if (responseCinemaRating is null)
                return NotFound();

            var responseUserRating = await _userService.UpdateRatingList(
                userId, 
                new UpdateUserRatingRequest {
                    CinemaId = request.Id,
                    Rating = request.Rating,
                }, 
                ct);


            return Ok(responseUserRating);
        }

        /// <summary>
        /// Update single user with provided request parameters
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="role">request role</param>
        /// <returns>Updated user instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpPut("{userId}/grant-role")]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GrantRoleAsync(
            [FromRoute] string userId,
            [FromQuery] string role,
            CancellationToken ct = default)
        {
            var response = await _accessService.GrantRole(userId, role, ct);

            return Ok(response);
        }

        /// <summary>
        /// Update single user with provided request parameters
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <param name="role">requested role</param>
        /// <returns>Updated user instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpPut("{userId}/revoke-role")]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeRoleAsync(
            [FromRoute] string userId,
            [FromQuery] string role,
            CancellationToken ct = default)
        {
            var response = await _accessService.RevokeRole(userId, role, ct);

            return Ok(response);
        }

        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="userId">id of user to be updated</param>
        /// <param name="request">request body with picture</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found or picture was not replaced</response>
        [HttpPut("{userId}/picture/update")]
        [ProducesResponseType(typeof(UpdatePictureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePicture(
            CancellationToken ct,
            [FromRoute] string userId,
            [FromBody] ReplaceImageRequest request
            )
        {
            string? pictureUri;

            var response = await _userService.Get(userId, ct);


            if (response.Picture is null)
            {
                pictureUri = await _imageService.AddImage(request.NewId, request.FileBase64, request.Size, ImageSize.Big);
            }
            else pictureUri = await _imageService.ReplaceImage(response.Picture, request.NewId, request.FileBase64, request.Size, ImageSize.Big);


            if (pictureUri == null)
            {
                return BadRequest(request);
            }

            UpdatePictureResponse? responsePhoto = await _userService.UpdatePhoto(userId, new UpdatePictureRequest { Picture = pictureUri }, ct);

            if (responsePhoto is null)
            {
                return BadRequest(userId);
            }

            if (responsePhoto.Picture != null)
            {
                responsePhoto.PictureUri = pictureUri;
            }

            return Ok(responsePhoto);
        }

        /// <summary>
        /// Delete single user by its id
        /// </summary>
        /// <param name="userId">authorized user Id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">User is not found</response>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string userId,
            CancellationToken ct = default
            )
        {
            var user = await _userService.Get(userId, ct);

            if (user is null)
            {
                return NotFound();
            }

            var responseProfile = await _accessService.DeleteProfile(userId, ct);

            if (!responseProfile)
            {
                return BadRequest();
            }

            var response = await _userService.Delete(userId, ct);

            if (!response)
            {
                return BadRequest();
            }

            if(user.Picture != null)
                await _imageService.DeleteImage(user.Picture, (ImageSize)31);

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
        [HttpDelete("{userId}/label/{cinemaId}/{label}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Labeled(
            [FromRoute] string userId,
            [FromRoute] string cinemaId,
            [FromRoute] Label label = Label.None,
            CancellationToken ct = default
            )
        {
            bool response;

            Label newLabel = Label.None;

            if (cinemaId != null && label != Label.None)
            {
                var responseLabel = await _userService.GetLabelFor(userId, cinemaId, ct);

                if (responseLabel == null || !responseLabel.Any())
                {
                    return BadRequest();
                }

                Label oldLabel = responseLabel.FirstOrDefault().Label;

                if (oldLabel != label) {
                    newLabel = oldLabel ^ label;

                    response = await _userService.CreateForLabeledList(
                        userId, 
                        new CreateLabeledRequest {
                            CinemaId = cinemaId,
                            Label = newLabel
                        }, 
                        ct
                        ) != null;

                }
                else response = await _userService.DeleteFromLabeledList(userId, cinemaId, ct);
            }
            else response = await _userService.DeleteFromLabeledList(userId, cinemaId, ct);

            if (!response)
            {
                return BadRequest();
            }

            return Ok(new LabeledResponse { UserId = userId, CinemaId = cinemaId, Label = newLabel });
        }

    }
}
