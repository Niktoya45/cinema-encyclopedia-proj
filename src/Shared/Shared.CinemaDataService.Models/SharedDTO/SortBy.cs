namespace Shared.CinemaDataService.Models.SharedDTO
{
    public record class SortBy(bool? order, string? field)
    {
        public static bool Ascending = true;

        public static bool Descending = false;
        public bool? Order { get; set; } = order;
        public string? Field { get; set; } = field;
    }
}
