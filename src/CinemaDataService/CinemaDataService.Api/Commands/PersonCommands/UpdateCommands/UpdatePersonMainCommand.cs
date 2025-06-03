using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.Shared;
using System.Xml.Linq;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
{

    public class UpdatePersonMainCommand : IRequest<PersonResponse>
    {

        public UpdatePersonMainCommand(
            string id,
            string name,
            DateOnly birthDate,
            Country country,
            Job jobs,
            string? description
            )
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            Country = country;
            Jobs = jobs;
            Description = description;
        }
        public string Id { get; }
        public string Name { get; }
        public DateOnly BirthDate { get; }
        public Country Country { get; }
        public Job Jobs { get; }
        public string? Description { get; }
    }
}