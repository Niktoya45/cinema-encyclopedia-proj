namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class CinemaStarringQuery:StarringQuery
    {
        public CinemaStarringQuery(string cinemaId, string starringId) : base(cinemaId, starringId) 
        {
        }
    }
}
