using CinemaDataService.Api.Queries.SharedQueries;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasSearchQuery : SearchQuery
    {
        public CinemasSearchQuery( string search, Pagination? pagination = null) : base(search, pagination) 
        { 
        }
    }
}
