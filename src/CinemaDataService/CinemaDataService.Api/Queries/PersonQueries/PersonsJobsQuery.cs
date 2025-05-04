using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Repositories.Utils;

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
