using System.Linq.Expressions;
using CinemaDataService.Domain.Aggregates.Base;
using CinemaDataService.Infrastructure.Context;
using CinemaDataService.Infrastructure.Pagination;
using CinemaDataService.Infrastructure.Sort;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CinemaDataService.Infrastructure.Repositories.Abstractions
{
    public abstract class EntityRepository<T>:IEntityRepository<T> where T : Entity
    {
        protected IMongoCollection<T> _collection { get; set; }
        public T Add(T entity)
        {
            _collection.InsertOne(entity);

            return entity;
        }
        public Task<List<T>?> Find(Pagination.Pagination? pg = default, Expression<Func<T, bool>>? query = null, SortBy? sort=default, CancellationToken ct = default)
        {
            pg ??= new Pagination.Pagination();
            var condition = Builders<T>.Filter.Where(query ?? (e => true));
            var notDeleted = Builders<T>.Filter.Where(e => e.IsDeleted);

            var sortByAdditional = sort is null ? null
                                        : sort.Order == SortBy.Ascending ? 
                                            Builders<T>.Sort.Ascending(sort.Field)
                                          : Builders<T>.Sort.Descending(sort.Field);

            var sortByTimeCreated = Builders<T>.Sort.Descending(e => e.CreatedAt);

            SortDefinition<T> sortBy = sortByAdditional is null ? sortByTimeCreated
                                    : Builders<T>.Sort.Combine(sortByAdditional, sortByTimeCreated);

            return _collection.Find(condition & notDeleted).Sort(sortBy).Skip(pg.Skip).Limit(pg.Take).ToListAsync(ct);
        }

        public Task<T?> FindOne(Expression<Func<T, bool>>? query = null, CancellationToken ct = default)
        {
            var condition = Builders<T>.Filter.Where(query ?? (e => true));
            var notDeleted = Builders<T>.Filter.Where(e => e.IsDeleted);

            var sortByTimeCreated = Builders<T>.Sort.Descending(e => e.CreatedAt);

            return _collection.Find(condition & notDeleted).Sort(sortByTimeCreated).FirstOrDefaultAsync(ct);
        }
        public async Task<T?> FindById(string id, CancellationToken ct = default)
        {
            return await FindOne(e => e.Id == id, ct);
        }
        public abstract Task<T?> Update(T entity, CancellationToken ct = default);

        public async Task<T?> Delete(string id, CancellationToken ct = default)
        {
            T? found = await FindById(id);

            if (found == null)
            {
                return null;
            }
            _collection.UpdateOne(
                Builders<T>.Filter.Where(e => e.Id == id), 
                Builders<T>.Update.Set(e => e.IsDeleted, true)
                );
            found.IsDeleted = true;

            return found;
        }
    }
}
