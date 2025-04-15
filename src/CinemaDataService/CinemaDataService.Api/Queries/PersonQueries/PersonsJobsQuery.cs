using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsJobsQuery:PersonsQuery
    {
        public PersonsJobsQuery(Job jobs, SortBy? sort = null, Pagination? pagination = null):base(sort, pagination)
        {
            Jobs = jobs;
        }
        public Job Jobs { get; }
    }
}
