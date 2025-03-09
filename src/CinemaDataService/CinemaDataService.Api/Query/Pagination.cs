using Microsoft.AspNetCore.Mvc;

namespace CinemaDataService.Api.Query
{
    public record Pagination
    {
        const int _max = 28;
        public int? Skip { get; set; } = 0;
        public int? Take { get; set; } = _max;
    }
}
