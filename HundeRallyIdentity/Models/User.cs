using Microsoft.AspNetCore.Identity;

namespace HundeRallyIdentity.Models
{
    public class User : IdentityUser
    {
        public string UserName { get; set; }
    }
}
