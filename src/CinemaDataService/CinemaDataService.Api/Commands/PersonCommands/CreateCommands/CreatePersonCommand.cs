using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.Shared;

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
            CinemaRecord[]? filmography,
            string? description
            )
		{
			Name = name;
            BirthDate = birthDate;
			Country = country;
			Jobs = jobs;
            Picture = picture;
			Filmography = filmography;
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