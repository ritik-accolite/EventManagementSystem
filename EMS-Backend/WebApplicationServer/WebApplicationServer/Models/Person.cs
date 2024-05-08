using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationServer.Models
{
    public class Person : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Password { get; set; }

        public string Role { get; set; } 
        
        public string? ResetPasswordToken {  get; set; }

        public DateTime ResetPasswordExpiry { get; set; }
    }
}



