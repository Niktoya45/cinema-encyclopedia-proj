using CinemaDataService.Infrastructure.Models.RecordDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.SharedCommands.CreateCommands
{
    public class CreateProductionStudioCommand:IRequest<ProductionStudioResponse>
    {
        public CreateProductionStudioCommand(
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
