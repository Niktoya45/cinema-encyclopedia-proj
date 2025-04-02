using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Commands.PersonCommands
{
	public class CreatePersonCommand : IRequest<PersonResponse>
	{

		public CreatePersonCommand(
            string name,
            DateOnly birthDate,
            int country,
            int jobs,
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
        public int Country { get; }
        public int Jobs { get; }
        public string? Picture { get; }
        public CinemaRecord[]? Filmography { get; }
        public string? Description { get; }
    }
}