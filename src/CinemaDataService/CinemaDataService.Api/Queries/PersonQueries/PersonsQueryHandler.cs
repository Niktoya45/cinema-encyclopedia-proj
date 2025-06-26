using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Api.Queries;
using CinemaDataService.Api.Queries.CinemaQueries;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsQueryHandler : IRequestHandler<PersonsQueryCommonWrapper, Page<PersonsResponse>>
    {
        IPersonRepository _repository;
        IMapper _mapper;
        public PersonsQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Page<PersonsResponse>> Handle(PersonsQueryCommonWrapper request, CancellationToken cancellationToken)
        {
            IEnumerable<Person>? persons;
            request.Query.Pg.Take += Pagination._add;

            switch (request.Query)
            {
                case PersonsIdQuery piq:

                    IList<Person> list = new List<Person>();

                    foreach (string id in piq.Ids)
                    {
                        list.Add(await _repository.FindById(id, cancellationToken) ?? throw new NotFoundException(id, "Persons"));
                    }

                    persons = list;

                    break;

                case PersonsJobsQuery pjq:
                    persons = await _repository.FindByJobs(pjq.Jobs, pjq.Pg, pjq.Sort, cancellationToken);
                    break;

                case PersonsCountryQuery pcq:
                    persons = await _repository.FindByCountry(pcq.Country, pcq.Pg, pcq.Sort, cancellationToken);
                    break;

                default:
                    persons = await _repository.Find(pg: request.Query.Pg, sort: request.Query.Sort, ct:cancellationToken);
                    break;
            }

            if (persons == null) {
                // handle
                throw new NotFoundException("Persons");
            }

            IEnumerable<PersonsResponse> response = _mapper.Map<IEnumerable<Person>, IEnumerable<PersonsResponse>>(persons);

            int countRequested = request.Query.Pg.Take - Pagination._add;
            bool isEnd = response.Count() <= countRequested;

            return new Page<PersonsResponse> { IsEnd = isEnd, Response = (isEnd ? response : response.Take(countRequested)) };
        }
    }
}
