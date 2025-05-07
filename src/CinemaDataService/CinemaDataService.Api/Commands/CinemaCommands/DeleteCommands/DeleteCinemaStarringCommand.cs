using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands
{
    public class DeleteCinemaStarringCommand:DeleteStarringCommand
    {
        public DeleteCinemaStarringCommand(string? parentId, string starringId) : base(parentId, starringId) { }
    }
}
