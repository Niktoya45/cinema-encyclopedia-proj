using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Api.Exceptions.UserExceptions;

namespace UserDataService.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case RecordAdditionFailedException raf:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;

                    case InfrastructureException ie:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    default:
                        _logger.LogError("System error occurred. Message: {message}. Inner exception: {innerException}. Stack trace: {stackTrace}",
                            e.Message,
                            e.InnerException?.Message,
                            e.StackTrace);
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }
            }
        }
    }
}
