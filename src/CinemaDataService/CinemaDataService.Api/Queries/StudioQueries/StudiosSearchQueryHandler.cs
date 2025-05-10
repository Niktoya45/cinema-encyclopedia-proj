using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosSearchQueryHandler : IRequestHandler<StudiosSearchQuery, IEnumerable<SearchResponse>>
    {
        IStudioRepository _repository;
        IMapper _mapper;
        public StudiosSearchQueryHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchResponse>> Handle(StudiosSearchQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Studio>? studios = await _repository.FindByName(tokens: request.Search, pg: request.Pg, ct: cancellationToken);


            if (studios == null)
            {
                // handle
                throw new NotFoundException("Studios");
            }

            return _mapper.Map<IEnumerable<Studio>, IEnumerable<SearchResponse>>(studios);
        }
    }
}
