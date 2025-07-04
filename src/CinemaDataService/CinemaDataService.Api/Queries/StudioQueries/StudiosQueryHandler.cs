using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Api.Queries.CinemaQueries;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Queries.StudioQueries
{
    public class StudiosQueryHandler : IRequestHandler<StudiosQueryCommonWrapper, Page<StudiosResponse>>
    {
        IStudioRepository _repository;
        IMapper _mapper;
        public StudiosQueryHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Page<StudiosResponse>> Handle(StudiosQueryCommonWrapper request, CancellationToken cancellationToken)
        {
            IEnumerable<Studio>? studios;
            request.Query.Pg.Take += Pagination._add;

            switch (request.Query) 
            {
                case StudiosIdQuery ciq:

                    IList<Studio> list = new List<Studio>();

                    foreach (string id in ciq.Ids)
                    {
                        list.Add(await _repository.FindById(id, cancellationToken) ?? throw new NotFoundException(id, "Studios"));
                    }

                    studios = list;

                    break;

                case StudiosSearchPageQuery sspq:
                    studios = await _repository.FindByName(sspq.Search.Split(), sspq.Pg, sspq.Sort, cancellationToken);
                    break;

                case StudiosYearQuery syq:
                    studios = await _repository.FindByYear(syq.Year, syq.Pg, syq.Sort, cancellationToken);
                    break;

                case StudiosYearSpansQuery sysq:
                    studios = await _repository.FindByYearSpans(sysq.YearsLower, sysq.YearSpan, sysq.Pg, sysq.Sort, cancellationToken);
                    break;

                case StudiosCountryQuery scq:
                    studios = await _repository.FindByCountry(scq.Country, scq.Pg, scq.Sort, cancellationToken);
                    break;

                default:
                    studios = await _repository.Find(pg:request.Query.Pg, sort:request.Query.Sort, ct:cancellationToken);
                    break;

            }

            if (studios == null) {
                // handle
                throw new NotFoundException("Studios");
            }

            IEnumerable<StudiosResponse> response = _mapper.Map<IEnumerable<Studio>, IEnumerable<StudiosResponse>>(studios);

            int countRequested = request.Query.Pg.Take - Pagination._add;
            bool isEnd = response.Count() <= countRequested;

            return new Page<StudiosResponse> { IsEnd = isEnd, Response = (isEnd ? response : response.Take(countRequested)) };
        }
    }
}
