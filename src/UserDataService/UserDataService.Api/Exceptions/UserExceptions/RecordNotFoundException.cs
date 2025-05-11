namespace UserDataService.Api.Exceptions.InfrastructureExceptions
{
    public class RecordNotFoundException:InfrastructureException
    {
        public RecordNotFoundException(string coll) : base($"No element was found by searched criteria; Searched collection: {coll};")
        {
            CollectionName = coll;
        }
        public RecordNotFoundException(string? userId, string? cinemaId, string coll) : base($"Element not found by UserId: {userId}, CinemaId: {cinemaId}; Searched collection: {coll};")
        {
            UserId = userId;
            CinemaId = cinemaId;
            CollectionName = coll;
        }
        public RecordNotFoundException(string? userId, string? cinemaId, string coll, string message) : base(message)
        {
            UserId = userId;
            CinemaId = cinemaId;
            CollectionName = coll;
        }
        public string? UserId { get; }
        public string? CinemaId { get; }
        public string CollectionName { get; }
    }
}
