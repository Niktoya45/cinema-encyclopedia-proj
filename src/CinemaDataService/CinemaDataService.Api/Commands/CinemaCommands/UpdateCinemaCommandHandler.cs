using AutoMapper;
using MediatR;
using CinemaDataService.Api.Exceptions.CinemaExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.CinemaCommands
{
    public class UpdateCinemaCommandHandler : IRequestHandler<UpdateCinemaCommand, CinemaResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public UpdateCinemaCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CinemaResponse> Handle(UpdateCinemaCommand request, CancellationToken cancellationToken)
        {
            Cinema Cinema = _mapper.Map<UpdateCinemaRequest, Cinema>(request);

            Cinema? updated = await _repository.Update(Cinema);

            if (updated == null)
            {
                // handle;
            }

            return _mapper.Map<Cinema, CinemaResponse>(updated);
        }
    }
}