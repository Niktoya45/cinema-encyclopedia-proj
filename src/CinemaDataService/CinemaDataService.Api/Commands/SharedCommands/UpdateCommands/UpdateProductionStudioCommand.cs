using CinemaDataService.Infrastructure.Models.RecordDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.SharedCommands.UpdateCommands
{
    public class UpdateProductionStudioCommand:IRequest<ProductionStudioResponse>
    {
        public UpdateProductionStudioCommand(
            string id,
            string name,
            string? picture
    )
        {
            Id = id;
            Name = name;
            Picture = picture;
        }
        public string Id { get; }
        public string Name { get; }
        public string? Picture { get; }
    }
}
