using AutoMapper;
using CinemaDataService.Api.Commands.PersonCommands.CreateCommands;
using CinemaDataService.Api.Commands.PersonCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;

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
                .ForMember(p => p.CreatedAt, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore());

            CreateMap<UpdatePersonMainCommand, Person>()
                .ForMember(p => p.Filmography, opt => opt.Ignore())
                .ForMember(p => p.Picture, opt => opt.Ignore())
                .ForMember(p => p.CreatedAt, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore());

            CreateMap<Person, PersonResponse>()
                .ForMember(pr => pr.PictureUri, opt => opt.Ignore());

            CreateMap<Person, PersonsResponse>()
                .ForMember(psr => psr.PictureUri, opt => opt.Ignore());

            CreateMap<Person, SearchResponse>()
                .ForMember(csr => csr.PictureUri, opt => opt.Ignore());
        }
    }
}
