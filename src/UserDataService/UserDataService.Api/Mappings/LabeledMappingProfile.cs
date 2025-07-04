using AutoMapper;
using UserDataService.Api.Commands.UserCommands.CreateCommands;
using UserDataService.Api.Commands.UserCommands.DeleteCommands;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.LabeledDTO;

namespace UserDataService.Api.Mappings
{
    public class LabeledMappingProfile : Profile
    {
        public LabeledMappingProfile()
        {
            CreateMap<CreateLabeledCommand, LabeledRecord>()
                .ForMember(r => r.Id, opt => opt.MapFrom(c => c.CinemaId))
                .ForMember(r => r.CinemaId, opt => opt.MapFrom(c => c.CinemaId))
                .ForMember(u => u.AddedAt, opt => opt.Ignore());
            
            CreateMap<DeleteLabeledCommand, LabeledRecord>()
                .ForMember(r => r.Id, opt => opt.MapFrom(c => c.CinemaId))
                .ForMember(r => r.CinemaId, opt => opt.MapFrom(c => c.CinemaId))
                .ForMember(u => u.AddedAt, opt => opt.Ignore());

            CreateMap<LabeledRecord, LabeledResponse>();
        }
    }
}
