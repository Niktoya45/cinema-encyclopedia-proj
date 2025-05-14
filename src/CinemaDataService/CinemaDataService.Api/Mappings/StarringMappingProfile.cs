using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.CreateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;
using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;
using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Api.Mappings
{
    public class StarringMappingProfile:Profile
    {
        public StarringMappingProfile() 
        {
            CreateMap<CreateStarringCommand, Starring>()
                .ForMember(s => s.ActorRole, 
                opt => opt.MapFrom(csc => 
                    new ActorRole {
                        Name = csc.RoleName, Priority = csc.RolePriority 
                    }
                ));

            CreateMap<CreateCinemaStarringCommand, Starring>()
                .IncludeBase<CreateStarringCommand, Starring>();

            CreateMap<UpdateStarringCommand, Starring>()
                    .ForMember(s => s.ActorRole,
                opt => opt.MapFrom(usc =>
                    new ActorRole
                    {
                        Name = usc.RoleName,
                        Priority = usc.RolePriority
                    }
                ));

            CreateMap<UpdateCinemaStarringCommand, Starring>()
                .IncludeBase<UpdateStarringCommand, Starring>();

            CreateMap<DeleteStarringCommand, Starring>();
            CreateMap<DeleteCinemaStarringCommand, Starring>()
                .IncludeBase<DeleteStarringCommand, Starring>();


            CreateMap<Starring, StarringResponse>()
                .ForMember(sr => sr.ParentId, opt => opt.Ignore())
                .ForMember(sr => sr.RoleName,
                opt => opt.MapFrom(s => s.ActorRole == null ? null : s.ActorRole.Name))
                .ForMember(sr => sr.RolePriority,
                opt => opt.MapFrom(s => s.ActorRole == null ? 0 : s.ActorRole.Priority))
                .ForMember(sr => sr.PictureUri, opt => opt.Ignore());

        }
    }
}
