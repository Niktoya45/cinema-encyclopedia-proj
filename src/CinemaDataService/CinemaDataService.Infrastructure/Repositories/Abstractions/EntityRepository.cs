using System.Linq.Expressions;
using CinemaDataService.Domain.Aggregates.Base;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Utils;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
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

        public async Task<E?> AddRecord<E>(Expression<Func<T, bool>> entity, Expression<Func<T, IEnumerable<E>?>> field, E record, Expression<Func<E, bool>>? excludeDuplicate = default, CancellationToken ct=default) where E : Value
        {
            UpdateDefinition<T> update = Builders<T>.Update.Push(field, record);
            FilterDefinition<T> notPresent = excludeDuplicate is null ?
                                              Builders<T>.Filter.Empty :
                                              Builders<T>.Filter.Not(
                                                  Builders<T>.Filter.ElemMatch<E>(field, excludeDuplicate)
                                                  );

            FilterDefinition<T> findEntity = Builders<T>.Filter.Where(entity);

            var res = await _collection.UpdateOneAsync(findEntity & notPresent, update, cancellationToken: ct);

            if (!res.IsAcknowledged)
            {
                return null;
            }

            return record;
        }

        public async Task<List<T>?> Find(Expression<Func<T, bool>>? query = null, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default)
        {
            var condition = Builders<T>.Filter.Where(query ?? (e => true));

            return await Find(condition, pg, sort, ct);
        }

        public async Task<List<T>?> Find(FilterDefinition<T> condition, Pagination? pg = default,  SortBy? sort=default, CancellationToken ct = default)
        {   
            pg ??= new Pagination(0, null);

            var notDeleted = Builders<T>.Filter.Where(e => !e.IsDeleted);

            var sortByAdditional = sort is null ? null : sort.Field is null || sort.Order is null ? null
                                        : sort.Order == SortBy.Ascending ? 
                                            Builders<T>.Sort.Ascending(sort.Field)
                                          : Builders<T>.Sort.Descending(sort.Field);

            var sortByTimeCreated = Builders<T>.Sort.Descending(e => e.CreatedAt);

            SortDefinition<T> sortBy = sortByAdditional is null ? sortByTimeCreated
                                    : Builders<T>.Sort.Combine(sortByAdditional, sortByTimeCreated);

            return await _collection.Find(condition & notDeleted).Sort(sortBy).Skip(pg.Skip).Limit(pg.Take).ToListAsync(ct);
        }

        public async Task<T?> FindOne(Expression<Func<T, bool>>? query = null, CancellationToken ct = default)
        {
            var condition = Builders<T>.Filter.Where(query ?? (e => true));
            var notDeleted = Builders<T>.Filter.Where(e => !e.IsDeleted);

            var sortByTimeCreated = Builders<T>.Sort.Descending(e => e.CreatedAt);

            return await _collection.Find(condition & notDeleted).Sort(sortByTimeCreated).FirstOrDefaultAsync(ct);
        }
        public async Task<T?> FindById(string id, CancellationToken ct = default)
        {
            return await FindOne(e => e.Id == id, ct);
        }

        public async Task<List<T>?> FindByName(string[] tokens, Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default)
        {
            List<BsonRegularExpression> exprs = new List<BsonRegularExpression>();

            foreach (string token in tokens) 
            {
                exprs.Add(new BsonRegularExpression(token, "i"));
            }

            FilterDefinition<T> condition = Builders<T>.Filter.In(e => e.Name, exprs);

            return await Find(condition, pg, sort, ct);
        }

        public async Task<T?> FindByName(string name, CancellationToken ct = default)
        {
            return await FindOne(e => e.Name == name, ct);
        }

        public abstract Task<T?> Update(T entity, CancellationToken ct = default);

        public async Task<T?> UpdatePicture(string id, string? picture, CancellationToken ct = default)
        {
            T? entity = await FindById(id, ct);

            var update = Builders<T>.Update.Set(e => e.Picture, picture);

            if (entity is null) {
                return null;
            }

            var find = Builders<T>.Filter.Where(e => e.Id == entity.Id);

            var res = await _collection.UpdateOneAsync(find, update, new UpdateOptions { IsUpsert = false }, ct); ;

            return entity;
        }

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
