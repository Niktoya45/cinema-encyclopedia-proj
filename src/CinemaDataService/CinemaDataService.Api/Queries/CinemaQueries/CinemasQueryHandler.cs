﻿using AutoMapper;
using MediatR;
using CinemaDataService.Api.Queries;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Queries.CinemaQueries
{
    public class CinemasQueryHandler : IRequestHandler<CinemasQuery, IEnumerable<CinemasResponse>>
    {
        ICinemaRepository _repository;
        IMapper _mapper;
        public CinemasQueryHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CinemasResponse>> Handle(CinemasQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Cinema>? cinemas = null;

            switch (request)
            {
                case CinemasYearQuery cyq:
                    cinemas = await _repository.FindByYear(cyq.Year, cyq.Pg, cyq.Sort, cancellationToken);
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
                    cinemas = await _repository.Find(pg:request.Pg, sort:request.Sort, ct:cancellationToken);
                    break;
            }

            if (cinemas == null) {
                // handle
                throw new NotFoundException("Cinemas");
            }

            return _mapper.Map<IEnumerable<Cinema>, IEnumerable<CinemasResponse>>(cinemas);
        }
    }
}
