using AutoMapper;
using UserDataService.Api.Commands.UserCommands.CreateCommands;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.RatingDTO;

namespace UserDataService.Api.Mappings
{
    public class RatingMappingProfile : Profile
    {
        public RatingMappingProfile()
        {
            CreateMap<CreateLabeledCommand, RatingRecord>()
                .ForMember(u => u.Id, opt => opt.Ignore())
                .ForMember(u => u.AddedAt, opt => opt.Ignore());

            CreateMap<RatingRecord, RatingResponse>();

            CreateMap<IEnumerable<RatingRecord>, IEnumerable<RatingResponse>>();
        }
    }
}
