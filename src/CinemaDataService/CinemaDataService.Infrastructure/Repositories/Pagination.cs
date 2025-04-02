
namespace CinemaDataService.Infrastructure.Pagination
{
    public record Pagination
    {
        const int _max = 49;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = _max;
        public Pagination()
        {

        }
        public Pagination(int? skip, int? take)
        {
            Skip = skip ?? 0;
            Take = take ?? _max;
        }
    }
}
