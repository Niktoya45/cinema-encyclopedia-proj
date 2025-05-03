namespace ImageService.Api.Exceptions
{
    public class ImageNotAddedException:Exception
    {
        public ImageNotAddedException() 
        {
        }

        public ImageNotAddedException(string message) : base(message) 
        { 
        }

        public ImageNotAddedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
