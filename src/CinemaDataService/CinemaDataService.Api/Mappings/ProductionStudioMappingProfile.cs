using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.CreateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands;
using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;
using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;
using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Mappings
{
    public class ProductionStudioMappingProfile:Profile
    {
        public ProductionStudioMappingProfile() 
        {
            CreateMap<CreateProductionStudioCommand, StudioRecord>();
            CreateMap<CreateCinemaProductionStudioCommand, StudioRecord>()
                .IncludeBase<CreateProductionStudioCommand, StudioRecord>();

            CreateMap<UpdateProductionStudioCommand, StudioRecord>();
            CreateMap<UpdateCinemaProductionStudioCommand, StudioRecord>()
                .IncludeBase<UpdateProductionStudioCommand, StudioRecord>();

            CreateMap<DeleteProductionStudioCommand, StudioRecord>();
            CreateMap<DeleteCinemaProductionStudioCommand, StudioRecord>()
                .IncludeBase<DeleteProductionStudioCommand, StudioRecord>();


            CreateMap<StudioRecord, ProductionStudioResponse>()
                .ForMember(ps => ps.ParentId, opt => opt.Ignore());
        }
    }
}
