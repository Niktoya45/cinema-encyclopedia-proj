using System;

namespace CinemaDataService.Infrastructure.Models.SharedDTO
{
    public class UpdateRatingRequest
    {
        public string Id { get; set; }
        public double Rating { get; set; }
    }
}
