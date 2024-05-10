using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Models;
namespace WebApplicationServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<Models.Person>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //public DbSet<Admin> admins { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<BookedEvent> BookedEvents { get; set; }

        public DbSet<Review> Reviews { get; set; }

       



    }
}
