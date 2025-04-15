using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.CinemaCommands.CreateCommands
{
    public class CreateCinemaStarringCommand:CreateStarringCommand
    {
        public CreateCinemaStarringCommand(
            string? cinemaId,
            string id,
            string name,
            Job jobs,
            string roleName,
            RolePriority rolePriority,
            string? picture
        ):base(id, name, jobs, roleName, rolePriority, picture)
        {
            CinemaId = cinemaId;
        }
        public string? CinemaId { get; }
    }
}
