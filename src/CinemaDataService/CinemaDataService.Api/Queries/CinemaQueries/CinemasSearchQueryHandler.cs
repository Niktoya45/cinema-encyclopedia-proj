using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasSearchQueryHandler : IRequestHandler<CinemasSearchQuery, IEnumerable<SearchResponse>>
    {
        ICinemaRepository _repository;
        IMapper _mapper;
        public CinemasSearchQueryHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchResponse>> Handle(CinemasSearchQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Cinema>? cinemas = await _repository.FindByName(tokens:request.Search, pg: request.Pg, ct: cancellationToken);


            if (cinemas == null)
            {
                // handle
                throw new NotFoundException("Cinemas");
            }

            return _mapper.Map<IEnumerable<Cinema>, IEnumerable<SearchResponse>>(cinemas);
        }
    }
}
