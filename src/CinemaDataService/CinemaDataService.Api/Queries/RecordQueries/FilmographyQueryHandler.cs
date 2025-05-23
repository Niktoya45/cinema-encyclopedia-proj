using AutoMapper;
using CinemaDataService.Api.Queries.PersonQueries;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using MediatR;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class FilmographyQueryHandler : IRequestHandler<FilmographyQuery, FilmographyResponse>
    {
        IPersonRepository _personRepository;
        IStudioRepository _studioRepository;
        IMapper _mapper;
        public FilmographyQueryHandler(IPersonRepository personRepository, IStudioRepository studioRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _studioRepository = studioRepository;
            _mapper = mapper;
        }
        public async Task<FilmographyResponse> Handle(FilmographyQuery request, CancellationToken cancellationToken)
        {
            CinemaRecord? filmography = null;
            string coll = "";

            switch (request)
            {

                case PersonFilmographyQuery pfq:
                    filmography = await _personRepository.FindFilmographyById(pfq.ParentId, pfq.Id, cancellationToken);
                    coll = "Persons";
                    break;

                case StudioFilmographyQuery sfq:
                    filmography = await _studioRepository.FindFilmographyById(sfq.ParentId, sfq.Id, cancellationToken);
                    coll = "Studios";
                    break;

                default:
                    break;
            }

            if (filmography == null) 
            {
                throw new NotFoundRecordException(request.ParentId, request.Id, coll);
            }


            return _mapper.Map<FilmographyResponse>(filmography);
        }
    }
}
