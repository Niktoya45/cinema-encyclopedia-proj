﻿using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class StarringQueryHandler : IRequestHandler<StarringQueryCommonWrapper, StarringResponse>
    {
        ICinemaRepository _cinemaRepository;
        IMapper _mapper;
        public StarringQueryHandler(ICinemaRepository cinemaRepository, IMapper mapper)
        {
            _cinemaRepository = cinemaRepository;
            _mapper = mapper;
        }
        public async Task<StarringResponse> Handle(StarringQueryCommonWrapper request, CancellationToken cancellationToken)
        {
            Starring? starring = null;

            switch (request.Query)
            {

                case CinemaStarringQuery csq:
                    starring = await _cinemaRepository.FindStarringById(csq.ParentId, csq.Id, cancellationToken);
                    break;

                default:
                    break;
            }

            if (starring == null)
            {
                throw new NotFoundRecordException(request.Query.ParentId, request.Query.Id, "Cinemas");
            }


            return _mapper.Map<StarringResponse>(starring);
        }
    }
}
