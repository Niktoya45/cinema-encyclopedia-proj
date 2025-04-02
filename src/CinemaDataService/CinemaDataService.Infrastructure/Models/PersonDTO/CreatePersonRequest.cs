
using CinemaDataService.Domain.Aggregates.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Infrastructure.Models.PersonDTO
{
    public class CreatePersonRequest
    {
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public int Country { get; set; }
        public int Jobs { get; set; }
        public string? Picture {  get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? Description { get; set; }
    }
}