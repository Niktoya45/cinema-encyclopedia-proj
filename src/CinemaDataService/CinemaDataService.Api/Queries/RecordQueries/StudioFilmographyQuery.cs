namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class StudioFilmographyQuery : FilmographyQuery
    {
        public StudioFilmographyQuery(string? studioId, string filmographyId) : base(studioId, filmographyId)
        {
        }
    }
}
