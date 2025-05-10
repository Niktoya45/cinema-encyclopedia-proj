namespace UserDataService.Api.Exceptions.Base
{
    public class UserDataException : Exception
    {
        public UserDataException():base(){}

        public UserDataException(string message) : base(message){}

        public UserDataException(string? message, Exception? innerException) : base(message, innerException){}

    }
}
