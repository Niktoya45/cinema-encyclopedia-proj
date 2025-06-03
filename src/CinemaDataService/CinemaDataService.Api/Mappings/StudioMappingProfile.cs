using AutoMapper;
using CinemaDataService.Api.Commands.StudioCommands.CreateCommands;
using CinemaDataService.Api.Commands.StudioCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Models.StudioDTO;

namespace CinemaDataService.Api.Mappings
{
    public class StudioMappingProfile:Profile
    {
        public StudioMappingProfile()
        {
            CreateMap<CreateStudioCommand, Studio>()
                .ForMember(s => s.Id, opt => opt.Ignore())
                .ForMember(s => s.CreatedAt, opt => opt.Ignore())
                .ForMember(s => s.IsDeleted, opt => opt.Ignore());

            CreateMap<UpdateStudioCommand, Studio>()
                .ForMember(s => s.Filmography, opt => opt.Ignore())
                .ForMember(s => s.Picture, opt => opt.Ignore())
                .ForMember(s => s.CreatedAt, opt => opt.Ignore())
                .ForMember(s => s.IsDeleted, opt => opt.Ignore());

            CreateMap<Studio, StudioResponse>()
                .ForMember(pr => pr.PictureUri, opt => opt.Ignore());

            CreateMap<Studio, StudiosResponse>()
                .ForMember(ssr => ssr.Year, opt => opt.MapFrom(s => s.FoundDate.Year))
                .ForMember(ssr => ssr.PictureUri, opt => opt.Ignore());

            CreateMap<Studio, SearchResponse>()
                .ForMember(csr => csr.PictureUri, opt => opt.Ignore());
            CreateMap<IEnumerable<Studio>, IEnumerable<SearchResponse>>();
        }
    }
}
