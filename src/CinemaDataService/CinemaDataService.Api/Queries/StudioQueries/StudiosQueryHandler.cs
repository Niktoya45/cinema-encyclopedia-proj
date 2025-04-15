using AutoMapper;
using MediatR;
using CinemaDataService.Api.Queries;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosQueryHandler : IRequestHandler<StudiosQuery, IEnumerable<StudiosResponse>>
    {
        IStudioRepository _repository;
        IMapper _mapper;
        public StudiosQueryHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudiosResponse>> Handle(StudiosQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Studio>? studios;

            switch (request) 
            {
                case StudiosYearQuery syq:
                    studios = await _repository.FindByYear(syq.Year, syq.Pg, syq.Sort, cancellationToken);
                    break;
                case StudiosCountryQuery scq:
                    studios = await _repository.FindByCountry(scq.Country, scq.Pg, scq.Sort, cancellationToken);
                    break;
                default:
                    studios = await _repository.Find(pg:request.Pg, sort:request.Sort, ct:cancellationToken);
                    break;

            }

            if (studios == null) {
                // handle
                throw new NotFoundException("Studios");
            }

            return _mapper.Map<IEnumerable<Studio>, IEnumerable<StudiosResponse>>(studios);
        }
    }
}
