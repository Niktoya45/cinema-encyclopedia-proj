using Humanizer.Localisation.TimeToClockNotation;
using System.Collections;

namespace EncyclopediaService.Api.Models
{
    public class CinemasViewModel
    {
        public int obj = 0;
        public int ColumnsCount = 4;
        public IList<CinemasModel> List { get; set; } = Enumerable.Range(1, 28).Select(x => new CinemasModel { Id = x.ToString() , Year=x+1994}).ToList();
    }

    public class CinemasModel{
        public string Id { get; set; }
        public int Year { get; set; }
    }
}
