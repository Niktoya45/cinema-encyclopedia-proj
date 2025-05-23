namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class PersonFilmographyQuery : FilmographyQuery
    {
        public PersonFilmographyQuery(string? personId, string filmographyId) : base(personId, filmographyId)
        {
        }
    }
}
