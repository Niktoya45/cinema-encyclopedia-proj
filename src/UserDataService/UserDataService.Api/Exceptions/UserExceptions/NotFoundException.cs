namespace UserDataService.Api.Exceptions.InfrastructureExceptions
{
    public class NotFoundException:InfrastructureException
    {
        public NotFoundException(string coll) : base($"No element was found by searched criteria; Searched collection: {coll};")
        {
            CollectionName = coll;
        }
        public NotFoundException(string? id, string coll) : base($"Element not found by Id: {id}; Searched collection: {coll};")
        {
            Id = id;
            CollectionName = coll;
        }
        public NotFoundException(string? id, string coll, string message) : base(message)
        {
            Id = id;
            CollectionName= coll;
        }
        public string? Id { get; }

        public string CollectionName { get; }
    }
}
