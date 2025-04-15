using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;
using System.Xml.Linq;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaProductionStudioCommand:UpdateProductionStudioCommand
    {
        public UpdateCinemaProductionStudioCommand(
            string id,
            string name,
            string? picture
        ):base(id, name, picture){}
    }
}
