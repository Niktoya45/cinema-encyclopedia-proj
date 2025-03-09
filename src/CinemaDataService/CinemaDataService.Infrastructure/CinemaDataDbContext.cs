using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using CinemaDataService.Domain.Aggregates.Cinema;
using CinemaDataService.Domain.Aggregates.Person;
using CinemaDataService.Domain.Aggregates.Studio;


namespace CinemaDataService.Infrastructure
{
    public class CinemaDataDb
    {
        public IMongoDatabase _mongodb { get; init; }
        public IMongoCollection<Cinema> Cinemas { get; init; }
        public IMongoCollection<Person> Persons { get; init; }
        public IMongoCollection<Studio> Studios { get; init; }

        public CinemaDataDb(IMongoDatabase mongodb)
        {
            _mongodb = mongodb;

            Cinemas = _mongodb.GetCollection<Cinema>("cinemas");
            Persons = _mongodb.GetCollection<Person>("persons");
            Studios = _mongodb.GetCollection<Studio>("studios");
        }
    }
}
