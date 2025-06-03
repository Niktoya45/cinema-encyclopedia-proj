using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosQueryCommonWrapper : IRequest<Page<StudiosResponse>>
    {

        public StudiosQueryCommonWrapper(StudiosQuery query) 
        {
            Query = query;
        }

        public StudiosQuery Query { get; }
    }
}
