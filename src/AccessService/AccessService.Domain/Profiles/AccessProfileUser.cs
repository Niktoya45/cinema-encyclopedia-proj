

using Microsoft.AspNetCore.Identity;

namespace AccessService.Domain.Profiles;

// Add profile data for application users by adding properties to the AccessProfileUser class
public class AccessProfileUser : IdentityUser
{
    public AccessProfileUser() : base()
    {
    }
    public AccessProfileUser(string username) : base(username)
    {
    }
}

