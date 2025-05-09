﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Api.Queries.StudioQueries;
using CinemaDataService.Api.Commands.StudioCommands.CreateCommands;
using CinemaDataService.Api.Commands.StudioCommands.UpdateCommands;
using CinemaDataService.Api.Commands.StudioCommands.DeleteCommands;
using CinemaDataService.Infrastructure.Repositories.Utils;

namespace CinemaDataService.Api.Controllers
{
    [ApiController]
    [Route("api/studios")]
    public class StudioController : Controller
    {

        private readonly IMediator _mediator;

        public StudioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all studios by optional sort and pagination criteria
        /// </summary>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new StudiosQuery(st, pg));

            return Ok(response);
        }

        /// <summary>
        /// Get all studios by provided year with optional sort and pagination criteria
        /// </summary>
        /// <param name="year">year of studio foundation</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("year/{year:int}")]
        [ProducesResponseType(typeof(IEnumerable<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Year(
            [FromRoute] int year,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new StudiosYearQuery(year, st, pg));

            return Ok(response);
        }

        /// <summary>
        /// Get all studios by provided country with optional sort and pagination criteria
        /// </summary>
        /// <param name="country">country of studio foundation</param>
        /// <param name="st">sort parameters</param>
        /// <param name="pg">pagination parameters</param>
        /// <returns>All studios list</returns>
        /// <response code="200">Success</response>
        /// <response code="400">No studio was found for this user</response>
        /// <response code="500">Something is wrong on a server</response>
        [HttpGet("country/{country}")]
        [ProducesResponseType(typeof(IEnumerable<StudiosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Country(
            [FromRoute] Country country,
            [FromQuery] SortBy? st = null,
            [FromQuery] Pagination? pg = null
            )
        {
            var response = await _mediator.Send(new StudiosCountryQuery(country, st, pg));

            return Ok(response);
        }

        /// <summary>
        /// Get studio by its id
        /// </summary>
        /// <param name="id">requested studio id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string id)
        {
            var response = await _mediator.Send(
                new StudioQuery(id)
                );

            return Ok(response);
        }

        /// <summary>
        /// Post new studio made of request parameter
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
        /// Add cinema to studio filmography made of request parameter
        /// </summary>
        /// <param name="studioId">studio id</param>
        /// <param name="request">request body</param>
        /// <returns>Newly created studio instance</returns>
        /// <response code="200">Success</response>
        [HttpPost("{studioId}/filmography")]
        [ProducesResponseType(typeof(FilmographyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Filmography(
            [FromRoute] string studioId,
            [FromBody] CreateFilmographyRequest request
            )
        {
            var response = await _mediator.Send(
                new CreateStudioFilmographyCommand(
                        studioId,
                        request.Id,
                        request.Name,
                        request.Year,
                        request.Picture
                    )
                );

            return Ok(response);
        }

        /// <summary>
        /// Update single studio with provided request parameters
        /// </summary>
        /// <param name="id">id of studio to be updated</param>
        /// <param name="request">request body</param>
        /// <returns>Updated task instance</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(
            [FromRoute] string id,
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
        /// Update filmography entrance for all studios
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
            [FromBody] UpdateFilmographyRequest request
            )
        {
            var response = await _mediator.Send(
                new UpdateStudioFilmographyCommand(
                        id,
                        request.Name,
                        request.Year,
                        request.Picture
                    )
                );

            return Ok(response);
        }

        /// <summary>
        /// Delete single studio by its name
        /// </summary>
        /// <param name="id">Id of studio to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id
            )
        {
            await _mediator.Send(new DeleteStudioCommand(id));

            return Ok();
        }

        /// <summary>
        /// Delete single cinema from filmography by optional studio id
        /// </summary>
        /// <param name="studioId">Id of studio which filmography cinema to be deleted</param>
        /// <param name="id">Id of filmography entrance</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Studio or cinema is not found</response>
        [HttpDelete("filmography/{id}")]
        [HttpDelete("{studioId}/filmography/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Filmography(
            [FromRoute] string? studioId,
            [FromRoute] string id
            )
        {
            await _mediator.Send(new DeleteStudioFilmographyCommand(studioId, id));

            return Ok();
        }
    }
}
