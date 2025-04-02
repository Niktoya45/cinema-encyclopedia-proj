using AutoMapper;
using MediatR;
using CinemaDataService.Api.Queries;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Api.Exceptions.PersonExceptions;
using CinemaDataService.Infrastructure.Repositories.UnitOfWork;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsQueryHandler : IRequestHandler<PersonsQuery, IEnumerable<GetPersonsResponse>>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public PersonsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            IUnitOfWork _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPersonsResponse>> Handle(PersonsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Person>? Persons;
            if (request.Email != null)
                Persons = await _unitOfWork.Persons.GetByEmail(request.Email, cancellationToken);
            else Persons = await _unitOfWork.Persons.GetAll(null, cancellationToken, request.Pg);

            if (Persons == null)
                throw new TrialPersonNotFoundException("No Person was found", request.Email);

            return _mapper.Map<IEnumerable<Person>, IEnumerable<GetPersonsResponse>>(Persons);
        }
    }
}
