using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace StudioDataService.Infrastructure.Repositories.Implementations
{
    public class StudioRepository:EntityRepository<Studio>, IStudioRepository
    {
        public override async Task<Studio?> Update(Studio entity, CancellationToken ct = default)
        {
            UpdateDefinitionBuilder<Studio> builder = Builders<Studio>.Update;

            UpdateDefinition<Studio> Set<TField>(Expression<Func<Studio, TField>> field, TField value) => builder.Set(field, value);

            var update = Builders<Studio>.Update.Combine(
                    Set(e => e.Name, entity.Name),
                    Set(e => e.FoundDate, entity.FoundDate),
                    Set(e => e.Country, entity.Country),
                    Set(e => e.Picture, entity.Picture),
                    Set(e => e.Filmography, entity.Filmography),
                    Set(e => e.PresidentName, entity.PresidentName),
                    Set(e => e.Description, entity.Description)
                );

            var find = Builders<Studio>.Filter.Where(e => e.Id == entity.Id);

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return entity;
        }
    }
}
