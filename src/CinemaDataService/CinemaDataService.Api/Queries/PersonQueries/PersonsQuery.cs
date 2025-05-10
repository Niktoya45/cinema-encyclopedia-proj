using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsQuery : IRequest<Page<PersonsResponse>>
    {
        public SortBy? Sort { get; }
        public Pagination Pg { get; }

        public PersonsQuery(SortBy? sort = null, Pagination? pagination = null, string? email=null)
        {
            Sort = sort;
            Pg = pagination ?? new Pagination(0, Pagination._max);
        }
    }
}
