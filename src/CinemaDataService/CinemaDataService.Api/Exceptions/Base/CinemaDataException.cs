namespace CinemaDataService.Api.Exceptions.Base
{
    public class CinemaDataException : Exception
    {
        public CinemaDataException():base(){}

        public CinemaDataException(string message) : base(message){}

        public CinemaDataException(string? message, Exception? innerException) : base(message, innerException){}

    }
}
