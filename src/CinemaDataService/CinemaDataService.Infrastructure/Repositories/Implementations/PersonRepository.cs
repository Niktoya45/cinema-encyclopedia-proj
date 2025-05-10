using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Context;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CinemaDataService.Infrastructure.Repositories.Implementations
{
    public class PersonRepository:EntityRepository<Person>, IPersonRepository
    {
        public PersonRepository(CinemaDataDb db)
        {
            _collection = db.Persons;
        }
        public async Task<CinemaRecord?> AddToFilmography(string? personId, CinemaRecord cinema, CancellationToken ct)
        {
            return await AddRecord( p => p.Id == personId,
                                    p => p.Filmography,
                                    cinema,
                                    c => c.Id == cinema.Id,
                                    ct
                                    );
        }
        public async Task<List<Person>?> FindByJobs(Job jobs, Pagination? pg = default, SortBy? st = default,  CancellationToken ct = default)
        {
            return await Find(p => (p.Jobs & jobs) != 0, 
                                pg, 
                                st, 
                                ct
                                );
        }
        public async Task<List<Person>?> FindByCountry(Country country, Pagination? pg = default, SortBy? st = default, CancellationToken ct = default)
        {
            return await Find(p => p.Country == country, 
                                pg, 
                                st, 
                                ct
                                );
        }
        public override async Task<Person?> Update(Person entity, CancellationToken ct = default)
        {
            UpdateDefinitionBuilder<Person> builder = Builders<Person>.Update;

            UpdateDefinition<Person> Set<TField>(Expression<Func<Person, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<Person>.Update.Combine(
                    Set(e => e.Name, entity.Name),
                    Set(e => e.BirthDate, entity.BirthDate),
                    Set(e => e.Country, entity.Country),
                    Set(e => e.Jobs, entity.Jobs),
                    Set(e => e.Picture, entity.Picture),
                    Set(e => e.Filmography, entity.Filmography),
                    Set(e => e.Description, entity.Description)
                );

            var find = Builders<Person>.Filter.Where(e => e.Id == entity.Id);

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return entity;
        }
        public async Task<CinemaRecord?> UpdateFilmography(CinemaRecord cinema, CancellationToken ct = default)
        {
            FilterDefinition<Person> elementMatch = Builders<Person>.Filter.ElemMatch(c => c.Filmography, s => s.Id == cinema.Id);
            UpdateDefinition<Person> replace = Builders<Person>.Update.Set("Starrings.$", cinema);

            var res = await _collection.UpdateManyAsync(elementMatch, replace, cancellationToken: ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }
        public async Task<CinemaRecord?> DeleteFromFilmography(string? personId, CinemaRecord cinema, CancellationToken ct = default)
        {
            FilterDefinition<Person> elementMatch = Builders<Person>.Filter.ElemMatch(p => p.Filmography, c => c.Id == cinema.Id);
            UpdateDefinition<Person> delete = Builders<Person>.Update.PopFirst("Filmography.$");

            UpdateResult res;

            if (personId is null)
            {
                res = await _collection.UpdateManyAsync(elementMatch, delete, cancellationToken: ct);
            }
            else
            {
                FilterDefinition<Person> find = Builders<Person>.Filter.Where(p => p.Id == personId);

                res = await _collection.UpdateOneAsync(find & elementMatch, delete, cancellationToken: ct);
            }

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return cinema;
        }
    }
}
