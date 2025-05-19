using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands
{
    public class DeleteCinemaProductionStudioCommand:DeleteProductionStudioCommand
    {
        public DeleteCinemaProductionStudioCommand(string? parentId, string starringId) : base(parentId, starringId) { }
    }
}
