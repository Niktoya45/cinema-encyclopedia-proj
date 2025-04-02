using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CinemaDataService.Infrastructure.Repositories.Implementations
{
    public class CinemaRepository:EntityRepository<Cinema>, ICinemaRepository
    {
        public override async Task<Cinema?> Update(Cinema entity, CancellationToken ct = default)
        {
            UpdateDefinitionBuilder<Cinema> builder = Builders<Cinema>.Update;

            UpdateDefinition<Cinema> Set<TField>(Expression<Func<Cinema, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<Cinema>.Update.Combine(
                    Set(e => e.Name, entity.Name),
                    Set(e => e.ReleaseDate, entity.ReleaseDate),
                    Set(e => e.Genres, entity.Genres),
                    Set(e => e.Language, entity.Language),
                    Set(e => e.Picture, entity.Picture),
                    Set(e => e.ProductionStudios, entity.ProductionStudios),
                    Set(e => e.Starrings, entity.Starrings),
                    Set(e => e.Description, entity.Description)
                );

            var find = Builders<Cinema>.Filter.Where(e => e.Id == entity.Id);

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return entity;
        }
    }
}
