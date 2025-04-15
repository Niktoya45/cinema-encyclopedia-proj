using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Sort;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasLanguageQuery:CinemasQuery
    {
        public CinemasLanguageQuery(Language language, SortBy? sort = null, Pagination? pagination = null):base(sort, pagination)
        {
            Language = language;
        }

        public Language Language { get; }
    }
}
