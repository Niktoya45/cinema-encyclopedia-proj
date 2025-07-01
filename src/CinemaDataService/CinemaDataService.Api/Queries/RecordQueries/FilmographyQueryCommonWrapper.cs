using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class FilmographyQueryCommonWrapper:RecordQueryCommonWrapper<FilmographyQuery, FilmographyResponse>
    {
        public FilmographyQueryCommonWrapper(FilmographyQuery query):base(query)
        {
        }
    }
}
