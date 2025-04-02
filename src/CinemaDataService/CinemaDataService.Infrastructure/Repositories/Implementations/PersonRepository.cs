using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace PersonDataService.Infrastructure.Repositories.Implementations
{
    public class PersonRepository:EntityRepository<Person>, IPersonRepository
    {
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
    }
}
