using AutoMapper;
using MediatR;
using CinemaDataService.Api.Queries;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsQueryHandler : IRequestHandler<PersonsQuery, Page<PersonsResponse>>
    {
        IPersonRepository _repository;
        IMapper _mapper;
        public PersonsQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Page<PersonsResponse>> Handle(PersonsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Person>? persons;

            switch (request)
            {
                case PersonsJobsQuery pjq:
                    persons = await _repository.FindByJobs(pjq.Jobs, pjq.Pg, pjq.Sort, cancellationToken);
                    break;
                case PersonsCountryQuery pcq:
                    persons = await _repository.FindByCountry(pcq.Country, pcq.Pg, pcq.Sort, cancellationToken);
                    break;
                default:
                    persons = await _repository.Find(pg: request.Pg, sort: request.Sort, ct:cancellationToken);
                    break;
            }

            if (persons == null) {
                // handle
                throw new NotFoundException("Persons");
            }

            IEnumerable<PersonsResponse> response = _mapper.Map<IEnumerable<Person>, IEnumerable<PersonsResponse>>(persons);

            int countRequested = request.Pg.Take - Pagination._add;
            bool isEnd = response.Count() <= countRequested;

            return new Page<PersonsResponse> { IsEnd = isEnd, Response = (isEnd ? response : response.Take(countRequested)) };
        }
    }
}
