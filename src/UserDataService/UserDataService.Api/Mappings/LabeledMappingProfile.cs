using AutoMapper;
using UserDataService.Api.Commands.UserCommands.CreateCommands;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.LabeledDTO;

namespace UserDataService.Api.Mappings
{
    public class LabeledMappingProfile : Profile
    {
        public LabeledMappingProfile()
        {
            CreateMap<CreateLabeledCommand, LabeledRecord>()
                .ForMember(u => u.Id, opt => opt.Ignore())
                .ForMember(u => u.AddedAt, opt => opt.Ignore());

            CreateMap<LabeledRecord, LabeledResponse>();

            CreateMap<IEnumerable<LabeledRecord>, IEnumerable<LabeledResponse>>();
        }
    }
}
