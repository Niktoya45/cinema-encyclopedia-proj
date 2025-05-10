using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.CreateCommands;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Mappings
{
    public class CinemaMappingProfile:Profile
    {
        public CinemaMappingProfile()
        {
            CreateMap<CreateCinemaCommand, Cinema>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Rating, opt => opt.Ignore())
                .ForMember(c => c.CreatedAt, opt => opt.Ignore())
                .ForMember(c => c.IsDeleted, opt => opt.Ignore());

            CreateMap<UpdateCinemaCommand, Cinema>()
                .ForMember(c => c.Rating, opt => opt.Ignore())
                .ForMember(c => c.CreatedAt, opt => opt.Ignore())
                .ForMember(c => c.IsDeleted, opt => opt.Ignore());

            CreateMap<Cinema, CinemaResponse>()
                .ForMember(cr => cr.PictureUri, opt => opt.Ignore())
                .ForMember(cr => cr.Rating, opt => opt.MapFrom(c => c.Rating));

            CreateMap<Cinema, CinemasResponse>()
                .ForMember(csr => csr.PictureUri, opt => opt.Ignore())
                .ForMember(csr => csr.Year, opt => opt.MapFrom(c => c.ReleaseDate.Year))
                .ForMember(csr => csr.Rating, opt => opt.MapFrom(c => c.Rating.Score));

            CreateMap<Cinema, SearchResponse>()
                .ForMember(csr => csr.PictureUri, opt => opt.Ignore());
            CreateMap<IEnumerable<Cinema>, IEnumerable<SearchResponse>>();
        }
    }
}
