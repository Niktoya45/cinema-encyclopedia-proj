using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsCountryQuery:PersonsQuery
    {
        public PersonsCountryQuery(Country country, SortBy? sort = null, Pagination? pagination = null):base(sort, pagination)
        {
            Country = country;
        }

        public Country Country { get; }
    }
}
