using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{

    public class UpdateStudioCommand : IRequest<StudioResponse>
    {

        public UpdateStudioCommand(
            string id, 
            string name,
            DateOnly foundDate,
            Country country,
            string? picture,
            CinemaRecord[]? filmography,
            string? presidentName,
            string? description
            )
        {
            Id = id;
            Name = name;
            FoundDate = foundDate;
            Country = country;
            Picture = picture;
            Filmography = filmography;
            PresidentName = presidentName;
            Description = description;
        }
        public string Id { get; }
        public string Name { get; }
        public DateOnly FoundDate { get; }
        public Country Country { get; }
        public string? Picture { get; }
        public CinemaRecord[]? Filmography { get; }
        public string? PresidentName { get; }
        public string? Description { get; }
    }
}