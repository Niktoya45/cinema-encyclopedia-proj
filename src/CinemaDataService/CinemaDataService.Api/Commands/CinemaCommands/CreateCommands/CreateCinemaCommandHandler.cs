using AutoMapper;
using MediatR;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.CinemaCommands.CreateCommands
{
    public class CreateCinemaCommandHandler : IRequestHandler<CreateCinemaCommand, CinemaResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public CreateCinemaCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CinemaResponse> Handle(CreateCinemaCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            Cinema cinema = _mapper.Map<CreateCinemaCommand, Cinema>(request);
            Cinema added = _repository.Add(cinema);

            return _mapper.Map<Cinema, CinemaResponse>(added);
        }
    }
}