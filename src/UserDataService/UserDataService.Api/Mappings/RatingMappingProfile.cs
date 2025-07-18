using AutoMapper;
using UserDataService.Api.Commands.UserCommands.UpdateCommands;
using UserDataService.Api.Commands.UserCommands.DeleteCommands;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.RatingDTO;

namespace UserDataService.Api.Mappings
{
    public class RatingMappingProfile : Profile
    {
        public RatingMappingProfile()
        {
            CreateMap<UpdateRatingCommand, RatingRecord>()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.CinemaId, opt => opt.MapFrom(c => c.CinemaId))
                .ForMember(r => r.AddedAt, opt => opt.Ignore());
            
            CreateMap<DeleteRatingCommand, RatingRecord>()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.CinemaId, opt => opt.MapFrom(c => c.CinemaId))
                .ForMember(r => r.AddedAt, opt => opt.Ignore());

            CreateMap<RatingRecord, RatingResponse>();

        }
    }
}
