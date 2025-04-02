using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Domain.Aggregates.StudioAggregate;


namespace CinemaDataService.Infrastructure.Context
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
