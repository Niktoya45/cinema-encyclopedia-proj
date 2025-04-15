using System.Linq.Expressions;
using CinemaDataService.Infrastructure.Sort;
using CinemaDataService.Domain.Aggregates.Base;

namespace CinemaDataService.Infrastructure.Repositories
{
    public interface IEntityRepository<T> where T:Entity
    {
        public T Add(T entity);
        public Task<E?> AddRecord<E>(Expression<Func<T, bool>> entity, Expression<Func<T, IEnumerable<E>?>> field, E record, Expression<Func<E, bool>>? mustNot = default, CancellationToken ct=default) where E:Value;
        public Task<List<T>?> Find(Expression<Func<T, bool>>? query = null, Pagination.Pagination? pg = default, SortBy? sort = default, CancellationToken ct = default);
        public Task<T?> FindOne(Expression<Func<T, bool>>? query = null, CancellationToken ct = default);
        public Task<T?> FindById(string id, CancellationToken ct = default);
        public Task<T?> Update(T entity, CancellationToken ct = default);
        public Task<T?> Delete(string id, CancellationToken ct = default);

    }
}
