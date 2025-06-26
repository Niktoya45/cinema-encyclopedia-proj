using EncyclopediaService.Api.Models.Display;
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.SharedDTO;

namespace EncyclopediaService.Api.Models.TestData
{
    public static class TestRecords
    {
        public const bool Used = true;

        public static IList<CinemaRecord> CinemasList = Enumerable.Range(1, 25).Select(
            x => new CinemaRecord { 
                Id = "" + x, Name = "Cinema " + x, 
                Year = x + 1994, Rating = ((x % 10) + 0.5) }
            ).ToList();

        public static IList<PersonRecord> PersonsList = Enumerable.Range(1, 25).Select(
            x => new PersonRecord { 
                Id = "" + x, 
                Name = "Person Name" + x, 
                Jobs = Job.Actor }
            ).ToList();

        public static IList<StudioRecord> StudiosList = Enumerable.Range(1, 25).Select(
            x => new StudioRecord { 
                Id = "" + x, 
                Name = $"Studio {x}" }
            ).ToList();

        public static Func<string, List<SearchResponse>> SearchList = (string search) => new List<SearchResponse> {
                    new SearchResponse { Id = "12", Name = " Search Name 12"},
                    new SearchResponse { Id = "13", Name = " Search Name 13"},
                    new SearchResponse { Id = "14", Name = " Search Name 14"},
                    new SearchResponse { Id = "15", Name = " Search Name 15"},
                };

        public static Func<string, SearchResponse> SearchRecord = (string search) => new SearchResponse { Id = "12", Name = " Search Name 12" };
    }
}
