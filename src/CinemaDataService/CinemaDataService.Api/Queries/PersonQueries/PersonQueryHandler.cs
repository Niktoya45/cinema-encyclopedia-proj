using AutoMapper;
using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Api.Exceptions.PersonExceptions;
using CinemaDataService.Infrastructure.Repositories.UnitOfWork;

namespace CinemaDataService.Api.Queries.PersonQueries
{

    public class PersonQueryHandler : IRequestHandler<PersonQuery, PersonResponse>
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public PersonQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PersonResponse> Handle(PersonQuery request, CancellationToken cancellationToken)
        {
            Person? Person = await _unitOfWork.Persons.GetById(request.Id, cancellationToken);

            if (Person == null)
                throw new TrialPersonNotFoundException(request.Id);


            return _mapper.Map<PersonResponse>(Person);
        }
    }
}
