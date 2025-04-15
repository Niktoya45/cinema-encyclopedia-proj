using AutoMapper;
using CinemaDataService.Api.Commands.PersonCommands.CreateCommands;
using CinemaDataService.Api.Commands.PersonCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.PersonDTO;

namespace CinemaDataService.Api.Mappings
{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile()
        {
            CreateMap<CreatePersonCommand, Person>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.CreatedAt, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore());

            CreateMap<UpdatePersonCommand, Person>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.CreatedAt, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore());

            CreateMap<Person, PersonResponse>();

        }
    }
}
