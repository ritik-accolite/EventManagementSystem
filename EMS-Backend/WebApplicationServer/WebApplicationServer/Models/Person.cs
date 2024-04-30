using Microsoft.AspNetCore.Identity;

namespace WebApplicationServer.Models
{
    public class Person : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }   
    }
}



