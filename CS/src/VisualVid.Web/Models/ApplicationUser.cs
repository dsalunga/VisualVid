using Microsoft.AspNetCore.Identity;

namespace VisualVid.Web.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    // Extended profile data from the Members table
    public Member? MemberProfile { get; set; }
}

public class ApplicationRole : IdentityRole<Guid>
{
}
