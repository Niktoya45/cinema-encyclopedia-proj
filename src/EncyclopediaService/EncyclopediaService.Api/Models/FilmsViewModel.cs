using Humanizer.Localisation.TimeToClockNotation;
using System.Collections;

namespace EncyclopediaService.Api.Models
{
    public class FilmsViewModel
    {
        public int obj = 0;
        public int ColumnsCount = 4;
        public IList<FilmsModel> List { get; set; } = Enumerable.Range(1, 28).Select(x => new FilmsModel { Id = x }).ToList();
    }

    public class FilmsModel{
        public int Id { get; set; }
    }
}
