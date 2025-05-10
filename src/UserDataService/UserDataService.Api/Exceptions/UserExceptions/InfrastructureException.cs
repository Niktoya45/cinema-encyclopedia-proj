using UserDataService.Api.Exceptions.Base;

namespace UserDataService.Api.Exceptions.InfrastructureExceptions
{
    public class InfrastructureException:UserDataException
    {
        public InfrastructureException():base() { }
        public InfrastructureException(string message) : base(message) { }
    }
}
