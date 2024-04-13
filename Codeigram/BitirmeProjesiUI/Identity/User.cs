using Microsoft.AspNetCore.Identity;

namespace BitirmeProjesiUI.Identity
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string ProfilePhoto { get; set; }

    }
}
