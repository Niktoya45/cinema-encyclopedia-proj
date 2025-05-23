namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class CinemaProductionStudioQuery : ProductionStudioQuery
    {
        public CinemaProductionStudioQuery(string? cinemaId, string starringId) : base(cinemaId, starringId)
        {
        }
    }
}
