using CinemaDataService.Api.Queries.SharedQueries;
using CinemaDataService.Infrastructure.Models.SharedDTO;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosSearchQuery : SearchQuery
    {
        public StudiosSearchQuery(string search, Pagination? pagination = null) : base(search, pagination)
        {
        }
    }
}
