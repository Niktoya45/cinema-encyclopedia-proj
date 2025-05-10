using AutoMapper;
using CinemaDataService.Api.Commands.PersonCommands.CreateCommands;
using CinemaDataService.Api.Commands.PersonCommands.DeleteCommands;
using CinemaDataService.Api.Commands.PersonCommands.UpdateCommands;
using CinemaDataService.Api.Commands.StudioCommands.CreateCommands;
using CinemaDataService.Api.Commands.StudioCommands.UpdateCommands;
using CinemaDataService.Api.Commands.StudioCommands.DeleteCommands;
using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;
using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;
using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Mappings
{
    public class FilmographyMappingProfile:Profile
    {
        public FilmographyMappingProfile()
        {
            CreateMap<CreateFilmographyCommand, CinemaRecord>();
            CreateMap<CreatePersonFilmographyCommand, CinemaRecord>()
                .IncludeBase<CreateFilmographyCommand, CinemaRecord>();
            CreateMap<CreateStudioFilmographyCommand, CinemaRecord>()
                .IncludeBase<CreateFilmographyCommand, CinemaRecord>();

            CreateMap<UpdateFilmographyCommand, CinemaRecord>();
            CreateMap<UpdatePersonFilmographyCommand, CinemaRecord>()
                .IncludeBase<UpdateFilmographyCommand, CinemaRecord>();
            CreateMap<UpdateStudioFilmographyCommand, CinemaRecord>()
                .IncludeBase<UpdateFilmographyCommand, CinemaRecord>();

            CreateMap<DeleteFilmographyCommand, CinemaRecord>();
            CreateMap<DeletePersonFilmographyCommand, CinemaRecord>()
                .IncludeBase<DeleteFilmographyCommand, CinemaRecord>();
            CreateMap<DeleteStudioFilmographyCommand, CinemaRecord>()
                .IncludeBase<DeleteFilmographyCommand, CinemaRecord>();


            CreateMap<CinemaRecord, FilmographyResponse>()
                .ForMember(fr => fr.ParentId, opt => opt.Ignore())
                .ForMember(fr => fr.PictureUri, opt => opt.Ignore());

        }
    }
}
