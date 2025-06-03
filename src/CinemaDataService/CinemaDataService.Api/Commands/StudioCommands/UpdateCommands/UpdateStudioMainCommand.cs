using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{

    public class UpdateStudioMainCommand : IRequest<StudioResponse>
    {

        public UpdateStudioMainCommand(
            string id, 
            string name,
            DateOnly foundDate,
            Country country,
            string? presidentName,
            string? description
            )
        {
            Id = id;
            Name = name;
            FoundDate = foundDate;
            Country = country;
            PresidentName = presidentName;
            Description = description;
        }
        public string Id { get; }
        public string Name { get; }
        public DateOnly FoundDate { get; }
        public Country Country { get; }
        public string? PresidentName { get; }
        public string? Description { get; }
    }
}