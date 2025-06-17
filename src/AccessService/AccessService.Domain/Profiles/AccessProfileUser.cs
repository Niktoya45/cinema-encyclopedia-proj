

using Microsoft.AspNetCore.Identity;

namespace AccessService.Domain.Profiles;

// Add profile data for application users by adding properties to the AccessProfileUser class
public class AccessProfileUser : IdentityUser
{
    public AccessProfileUser() : base()
    {
    }
    public AccessProfileUser(string email, string sub, string appUsername) : base(email)
    {
        Sub = sub;
        AppUsername = appUsername;
    }

    public string Sub { get; set; }
    public string AppUsername { get; set; }
}

