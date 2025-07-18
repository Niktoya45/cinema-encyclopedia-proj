using AutoMapper;
using DnsClient;
using MediatR;
using MongoDB.Bson;
using UserDataService.Api.Exceptions.UserExceptions;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.LabeledDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Commands.UserCommands.CreateCommands
{
    public class CreateLabeledCommandHandler : IRequestHandler<CreateLabeledCommand, LabeledResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public CreateLabeledCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<LabeledResponse> Handle(CreateLabeledCommand request, CancellationToken cancellationToken)
        {
            LabeledRecord labeled = _mapper.Map<CreateLabeledCommand, LabeledRecord>(request);

            labeled.Id = ObjectId.GenerateNewId(DateTime.Now).ToString();

            LabeledRecord? added = await _repository.AddToLabeledList(labeled, cancellationToken);

            if (added is null)
            {
                throw new RecordAdditionFailedException("labeled_records");
            }

            return _mapper.Map<LabeledRecord, LabeledResponse>(added);
        }
    }
}