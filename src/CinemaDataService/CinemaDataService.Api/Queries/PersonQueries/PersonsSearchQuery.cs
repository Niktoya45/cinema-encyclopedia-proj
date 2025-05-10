using CinemaDataService.Api.Queries.SharedQueries;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsSearchQuery: SearchQuery
    {
        public PersonsSearchQuery(string search, Pagination? pagination = null) : base(search, pagination)
        {
        }
    }
}
