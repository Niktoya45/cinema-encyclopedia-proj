using CinemaDataService.Api.Exceptions.Base;

namespace CinemaDataService.Api.Exceptions.InfrastructureExceptions
{
    public class InfrastructureException:CinemaDataException
    {
        public InfrastructureException():base() { }
        public InfrastructureException(string message) : base(message) { }
    }
}
