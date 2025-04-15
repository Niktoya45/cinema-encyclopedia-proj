using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaStarringCommand:UpdateStarringCommand
    {
        public UpdateCinemaStarringCommand(
            string? cinemaId,
            string id,
            string name,
            Job jobs,
            string roleName,
            RolePriority rolePriority,
            string? picture
        ):base(id, name, jobs, roleName, rolePriority, picture) { }

        public string? CinemaId { get; }
    }
}
