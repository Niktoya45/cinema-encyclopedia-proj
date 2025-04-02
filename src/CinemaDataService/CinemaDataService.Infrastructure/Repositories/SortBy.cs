
namespace CinemaDataService.Infrastructure.Sort
{
    public record class SortBy
    {
        public static bool Ascending = true;

        public static bool Descending = false;
        public bool Order { get; set; }
        public string Field { get; set; }
        public SortBy(bool order, string field)
        {
            Order = order;
            Field = field;
        }
    }
}
