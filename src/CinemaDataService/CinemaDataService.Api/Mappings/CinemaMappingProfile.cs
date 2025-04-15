using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.CreateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;

namespace CinemaDataService.Api.Mappings
{
    public class CinemaMappingProfile:Profile
    {
        public CinemaMappingProfile()
        {
            CreateMap<CreateCinemaCommand, Cinema>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.RatingScore, opt => opt.Ignore())
                .ForMember(c => c.CreatedAt, opt => opt.Ignore())
                .ForMember(c => c.IsDeleted, opt => opt.Ignore());

            CreateMap<UpdateCinemaCommand, Cinema>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.RatingScore, opt => opt.Ignore())
                .ForMember(c => c.CreatedAt, opt => opt.Ignore())
                .ForMember(c => c.IsDeleted, opt => opt.Ignore());

            CreateMap<Cinema, CinemaResponse>();

        }
    }
}
