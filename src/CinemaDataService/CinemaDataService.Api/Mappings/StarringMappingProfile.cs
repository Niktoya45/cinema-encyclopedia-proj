using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.CreateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;
using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;
using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Mappings
{
    public class StarringMappingProfile:Profile
    {
        public StarringMappingProfile() 
        {
            CreateMap<CreateStarringCommand, Starring>();
            CreateMap<CreateCinemaStarringCommand, Starring>()
                .IncludeBase<CreateStarringCommand, Starring>();

            CreateMap<UpdateStarringCommand, Starring>();
            CreateMap<UpdateCinemaStarringCommand, Starring>()
                .IncludeBase<UpdateStarringCommand, Starring>();

            CreateMap<DeleteStarringCommand, Starring>();
            CreateMap<DeleteCinemaStarringCommand, Starring>()
                .IncludeBase<DeleteStarringCommand, Starring>();


            CreateMap<Starring, StarringResponse>()
                .ForMember(ps => ps.ParentId, opt => opt.Ignore());

        }
    }
}
