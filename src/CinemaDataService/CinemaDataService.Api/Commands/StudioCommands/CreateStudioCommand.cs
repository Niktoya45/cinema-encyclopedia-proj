using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.StudioCommands
{
	public class CreateStudioCommand : IRequest<StudioResponse>
	{

		public CreateStudioCommand(
            string name,
            DateOnly foundDate,
            int country,
            string? picture,
            CinemaRecord[]? filmography,
            string? presidentName,
            string? description
            )
        {
            Name = name;
            FoundDate = foundDate;
            Country = country;
            Picture = picture;
            Filmography = filmography;
            PresidentName = presidentName;
            Description = description;
        }

        public string Name { get; }
        public DateOnly FoundDate { get; }
        public int Country { get; }
        public string? Picture { get; }
        public CinemaRecord[]? Filmography { get; }
        public string? PresidentName { get; }
        public string? Description { get; }
    }
}