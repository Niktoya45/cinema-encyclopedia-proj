﻿

namespace CinemaDataService.Infrastructure.Models.SharedDTO
{
    public class FilmographyResponse
    {
        public string ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
