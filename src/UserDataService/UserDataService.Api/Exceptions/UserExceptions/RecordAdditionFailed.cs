using UserDataService.Api.Exceptions.InfrastructureExceptions;

namespace UserDataService.Api.Exceptions.UserExceptions
{
    public class RecordAdditionFailedException : InfrastructureException
    {
        public RecordAdditionFailedException(string coll) : base($"Record addition failed for collection: {coll};")
        {
            CollectionName = coll;
        }
        public string CollectionName { get; }
    }
}
