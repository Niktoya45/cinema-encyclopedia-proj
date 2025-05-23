namespace CinemaDataService.Api.Exceptions.InfrastructureExceptions
{
    public class NotFoundRecordException:InfrastructureException
    {
        public NotFoundRecordException(string coll) : base($"No element was found by searched criteria; Searched collection: {coll};")
        {
            CollectionName = coll;
        }
        public NotFoundRecordException(string? parentId, string? id, string coll) : base($"Record by Id: {id} not found for element by ParentId {parentId}; Searched collection: {coll};")
        {
            Id = id;
            ParentId = parentId;
            CollectionName = coll;
        }
        public NotFoundRecordException(string? parentId, string? id, string coll, string message) : base(message)
        {
            Id = id;
            CollectionName = coll;
            ParentId = parentId;
        }
        public string? Id { get; }
        public string? ParentId { get; }

        public string CollectionName { get; }
    }
}
