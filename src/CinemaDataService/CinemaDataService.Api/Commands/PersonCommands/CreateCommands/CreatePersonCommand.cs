using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;

namespace CinemaDataService.Api.Commands.PersonCommands.CreateCommands
{
	public class CreatePersonCommand : IRequest<PersonResponse>
	{

		public CreatePersonCommand(
            string name,
            DateOnly birthDate,
            Country country,
            Job jobs,
            string? picture,
            CreateFilmographyRequest[]? filmography,
            string? description
            )
		{
			Name = name;
            BirthDate = birthDate;
			Country = country;
			Jobs = jobs;
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
            Description = description;
		}

        public string Name { get; }
        public DateOnly BirthDate { get; }
        public Country Country { get; }
        public Job Jobs { get; }
        public string? Picture { get; }
        public CinemaRecord[]? Filmography { get; }
        public string? Description { get; }
    }
}