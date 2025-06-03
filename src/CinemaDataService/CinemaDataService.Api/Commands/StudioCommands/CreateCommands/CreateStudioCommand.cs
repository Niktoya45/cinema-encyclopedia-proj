using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Api.Commands.StudioCommands.CreateCommands
{
	public class CreateStudioCommand : IRequest<StudioResponse>
	{

		public CreateStudioCommand(
            string name,
            DateOnly foundDate,
            Country country,
            string? picture,
            CreateFilmographyRequest[]? filmography,
            string? presidentName,
            string? description
            )
        {
            Name = name;
            FoundDate = foundDate;
            Country = country;
            Picture = picture;
            Filmography = filmography is null ? null
                : filmography.Select(sr =>
                new CinemaRecord
                {
                    Id = sr.Id,
                    Name = sr.Name,
                    Year = sr.Year,
                    Picture = sr.Picture
                }
                ).ToArray();
            PresidentName = presidentName;
            Description = description;
        }

        public string Name { get; }
        public DateOnly FoundDate { get; }
        public Country Country { get; }
        public string? Picture { get; }
        public CinemaRecord[]? Filmography { get; }
        public string? PresidentName { get; }
        public string? Description { get; }
    }
}