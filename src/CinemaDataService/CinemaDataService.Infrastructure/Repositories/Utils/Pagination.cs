namespace CinemaDataService.Infrastructure.Repositories.Utils
{
    public record Pagination(int? skip, int? take)
    {
        public const int _max = 49;
        public int Skip { get; set; } = skip ?? 0;
        public int Take { get; set; } = take == null ? _max : take > _max ? _max : take??_max;
    }
}
