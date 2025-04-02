
using CinemaDataService.Domain.Aggregates.PersonAggregate;

namespace CinemaDataService.Infrastructure.Repositories.Abstractions
{
    public interface IPersonRepository:IEntityRepository<Person>
    {
    }
}
