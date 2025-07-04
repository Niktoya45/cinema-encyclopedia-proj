using AutoMapper;
using MediatR;
using CinemaDataService.Api.Queries;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasQueryHandler : IRequestHandler<CinemasQueryCommonWrapper, Page<CinemasResponse>>
    {
        ICinemaRepository _repository;
        IMapper _mapper;
        public CinemasQueryHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Page<CinemasResponse>> Handle(CinemasQueryCommonWrapper request, CancellationToken cancellationToken)
        {
            IEnumerable<Cinema>? cinemas = null;
            request.Query.Pg.Take += Pagination._add;

            switch (request.Query)
            {
                case CinemasIdQuery ciq:

                    IList<Cinema> list = new List<Cinema>();

                    foreach (string id in ciq.Ids) 
                    {
                        list.Add(await _repository.FindById(id, cancellationToken)?? throw new NotFoundException(id, "Cinemas") );
                    }

                    cinemas = list;

                    break;

                case CinemasSearchPageQuery cspq:
                    cinemas = await _repository.FindByName(cspq.Search.Split(), cspq.Pg, cspq.Sort, cancellationToken);
                    break;

                case CinemasYearQuery cyq:
                    cinemas = await _repository.FindByYear(cyq.Year, cyq.Pg, cyq.Sort, cancellationToken);
                    break;

                case CinemasYearSpansQuery cysq:
                    cinemas = await _repository.FindByYearSpans(cysq.YearsLower, cysq.YearSpan, cysq.Pg, cysq.Sort, cancellationToken);
                    break;

                case CinemasGenresQuery cgq:
                    cinemas = await _repository.FindByGenres(cgq.Genres, cgq.Pg, cgq.Sort, cancellationToken);
                    break;

                case CinemasLanguageQuery clq:
                    cinemas = await _repository.FindByLanguage(clq.Language, clq.Pg, clq.Sort, cancellationToken);
                    break;

                case CinemasStudioQuery csq:
                    cinemas = await _repository.FindByStudioId(csq.StudioId, csq.Pg, csq.Sort, cancellationToken);
                    break;

                default:
                    cinemas = await _repository.Find(pg:request.Query.Pg, sort:request.Query.Sort, ct:cancellationToken);
                    break;
            }

            if (cinemas == null) {
                // handle
                throw new NotFoundException("Cinemas");
            }

            IEnumerable<CinemasResponse> response = _mapper.Map<IEnumerable<Cinema>, IEnumerable<CinemasResponse>>(cinemas);

            int countRequested = request.Query.Pg.Take - Pagination._add;
            bool isEnd = response.Count() <= countRequested;

            return new Page<CinemasResponse> { IsEnd =  isEnd, Response = (isEnd ? response : response.Take(countRequested)) };
        }
    }
}
