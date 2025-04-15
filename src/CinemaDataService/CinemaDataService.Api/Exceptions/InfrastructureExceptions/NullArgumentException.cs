namespace CinemaDataService.Api.Exceptions.InfrastructureExceptions
{
    public class NullArgumentException:InfrastructureException
    {
        public NullArgumentException():base("Null id provided") { }
    }
}
